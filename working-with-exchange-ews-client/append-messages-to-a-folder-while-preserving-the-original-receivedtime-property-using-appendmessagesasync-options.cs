using System;
using System.Collections.Generic;
using System.Net;
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
            // Create the EWS client (connection safety wrapped in try/catch)
            IAsyncEwsClient client;
            try
            {
                // Replace with actual Exchange server URL and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = (IAsyncEwsClient)EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Prepare mail messages preserving their original ReceivedTime (Date property)
            List<MailMessage> messages = new List<MailMessage>();

            MailMessage message1 = new MailMessage(
                "alice@example.com",
                "bob@example.com",
                "Project Update",
                "Please find the latest project update attached.");
            // Preserve original received time
            message1.Date = new DateTime(2023, 3, 15, 9, 30, 0, DateTimeKind.Utc);
            messages.Add(message1);

            MailMessage message2 = new MailMessage(
                "carol@example.com",
                "dave@example.com",
                "Meeting Invitation",
                "You are invited to a meeting tomorrow at 10 AM.");
            // Preserve original received time
            message2.Date = new DateTime(2023, 3, 14, 14, 45, 0, DateTimeKind.Utc);
            messages.Add(message2);

            // Build the AppendMessage request, specifying the target folder
            EwsAppendMessage appendRequest = EwsAppendMessage.Create()
                .SetFolder("Inbox")
                .AddMessages(messages);

            // Append the messages asynchronously
            IEnumerable<string> appendedItemUris;
            try
            {
                appendedItemUris = await client.AppendMessagesAsync(appendRequest);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to append messages: {ex.Message}");
                return;
            }

            // Output the URIs of the newly created items
            foreach (string uri in appendedItemUris)
            {
                Console.WriteLine($"Appended message URI: {uri}");
            }

            // Ensure the client is properly disposed
            client.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
