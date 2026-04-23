using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages to process.");
                        return;
                    }

                    // Take the first message for processing
                    ExchangeMessageInfo messageInfo = messages[0];

                    // Fetch the full message
                    using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                    {
                        Console.WriteLine($"Processing message: {message.Subject}");
                        // Add any processing logic here
                    }

                    // Define the destination folder URI (ensure the folder exists on the server)
                    string destinationFolderUri = client.MailboxInfo.InboxUri + "/Processed";

                    // Move the processed message to the destination folder
                    client.MoveMessage(messageInfo, destinationFolderUri);
                    Console.WriteLine("Message moved successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
