using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration (replace placeholders with real values)
            string localDraftsFolder = "LocalDrafts";
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Ensure local drafts folder exists
            if (!Directory.Exists(localDraftsFolder))
                Directory.CreateDirectory(localDraftsFolder);

            // Create a minimal placeholder draft if folder is empty
            if (!Directory.EnumerateFiles(localDraftsFolder, "*.eml").Any())
            {
                var placeholder = new MailMessage("placeholder@example.com", "recipient@example.com",
                    "Placeholder Draft", "This is a placeholder draft.");
                string placeholderPath = Path.Combine(localDraftsFolder, "placeholder.eml");
                placeholder.Save(placeholderPath);
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl,
                new System.Net.NetworkCredential(username, password)))
            {
                // Get Drafts folder URI
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                if (string.IsNullOrEmpty(draftsFolderUri))
                {
                    Console.Error.WriteLine("Drafts folder URI not found.");
                    return;
                }

                // Retrieve server drafts
                ExchangeMessageInfoCollection serverDrafts = client.ListMessages(draftsFolderUri);
                HashSet<string> serverDraftUris = new HashSet<string>(serverDrafts.Select(m => m.UniqueUri));

                // Upload local drafts that are not already on the server
                foreach (string emlPath in Directory.EnumerateFiles(localDraftsFolder, "*.eml"))
                {
                    try
                    {
                        MailMessage localMessage = MailMessage.Load(emlPath);
                        bool existsOnServer = serverDrafts.Any(s =>
                            string.Equals(s.Subject, localMessage.Subject, StringComparison.OrdinalIgnoreCase));
                        if (existsOnServer)
                            continue;

                        string createdUri = client.AppendMessage(draftsFolderUri,
                            MapiMessage.FromMailMessage(localMessage), false);
                        serverDraftUris.Add(createdUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process local draft '{emlPath}': {ex.Message}");
                    }
                }

                // Build set of local draft subjects for deletion comparison
                HashSet<string> localDraftSubjects = new HashSet<string>(
                    Directory.EnumerateFiles(localDraftsFolder, "*.eml")
                             .Select(p =>
                             {
                                 try { return MailMessage.Load(p).Subject; }
                                 catch { return null; }
                             })
                             .Where(s => s != null));

                // Delete server drafts that no longer exist locally
                foreach (ExchangeMessageInfo serverDraft in serverDrafts)
                {
                    try
                    {
                        if (!localDraftSubjects.Contains(serverDraft.Subject))
                        {
                            client.DeleteItem(serverDraft.UniqueUri,
                                new DeletionOptions(DeletionType.MoveToDeletedItems));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete server draft '{serverDraft.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
