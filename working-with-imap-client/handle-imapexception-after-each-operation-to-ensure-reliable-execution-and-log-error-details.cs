using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use ImapClient
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select INBOX folder
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"SelectFolder failed: {imapEx.Message}");
                    Console.Error.WriteLine($"Details: {imapEx.ErrorDetails}");
                    return;
                }

                // List subfolders (optional)
                try
                {
                    ImapFolderInfoCollection subfolders = client.ListFolders("INBOX");
                    // Subfolders can be processed here if needed
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"ListFolders failed: {imapEx.Message}");
                    Console.Error.WriteLine($"Details: {imapEx.ErrorDetails}");
                }

                // List messages in the selected folder
                ImapMessageInfoCollection messages = null;
                try
                {
                    messages = client.ListMessages();
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"ListMessages failed: {imapEx.Message}");
                    Console.Error.WriteLine($"Details: {imapEx.ErrorDetails}");
                }

                if (messages != null && messages.Count > 0)
                {
                    // Fetch the first message
                    MailMessage fetchedMessage = null;
                    try
                    {
                        fetchedMessage = client.FetchMessage(messages[0].UniqueId);
                        Console.WriteLine($"Subject: {fetchedMessage.Subject}");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"FetchMessage failed: {imapEx.Message}");
                        Console.Error.WriteLine($"Details: {imapEx.ErrorDetails}");
                    }
                    finally
                    {
                        fetchedMessage?.Dispose();
                    }

                    // Delete the first message (mark as deleted and commit)
                    try
                    {
                        client.DeleteMessage(messages[0].UniqueId, true);
                        Console.WriteLine("Message deleted successfully.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"DeleteMessage failed: {imapEx.Message}");
                        Console.Error.WriteLine($"Details: {imapEx.ErrorDetails}");
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
