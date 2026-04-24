using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials and mailbox URI
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example") || username.Contains("example") || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Path to the .eml file to be uploaded
            string emlPath = "email.eml";

            // Ensure the .eml file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    var placeholder = new MailMessage("from@example.com", "to@example.com", "Placeholder Subject", "Placeholder body");
                    placeholder.Save(emlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .eml file: {ex.Message}");
                    return;
                }
            }

            // Load the email message from the .eml file
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load .eml file: {ex.Message}");
                return;
            }

            // Create an asynchronous EWS client
            IAsyncEwsClient asyncClient;
            try
            {
                asyncClient = await EWSClient.GetEwsClientAsync(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (asyncClient as IDisposable)
            {
                // Prepare the AppendMessage request
                EwsAppendMessage appendMessage = EwsAppendMessage.Create();
                appendMessage.AddMessage(MapiMessage.FromMailMessage(mailMessage));
                appendMessage.SetFolder("Inbox"); // Target folder

                // Append the message asynchronously
                try
                {
                    var appendResult = await asyncClient.AppendMessagesAsync(appendMessage);
                    foreach (var uri in appendResult)
                    {
                        Console.WriteLine($"Message uploaded. URI: {uri}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append message: {ex.Message}");
                    return;
                }

                // -----------------------------------------------------------------
                // Placeholder for SharePoint list update.
                // In a real scenario, you would use SharePoint client libraries or
                // Microsoft Graph to update the list with the email subject and a
                // link to the stored .eml file.
                // -----------------------------------------------------------------
                string emailSubject = mailMessage.Subject;
                string emlLink = $"https://sharepoint.example.com/Documents/{Path.GetFileName(emlPath)}";

                Console.WriteLine($"Would update SharePoint list with Subject: \"{emailSubject}\" and Link: {emlLink}");
                // Example (pseudo-code):
                // await sharePointClient.UpdateListItemAsync(listId, new { Title = emailSubject, FileUrl = emlLink });
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
