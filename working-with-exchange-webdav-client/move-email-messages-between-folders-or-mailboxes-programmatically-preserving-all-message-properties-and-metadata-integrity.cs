using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the Exchange client inside a using block as required
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                // Define source and destination folder URIs (using standard folders as example)
                string sourceFolderUri = client.MailboxInfo.InboxUri;
                string destinationFolderUri = client.MailboxInfo.DraftsUri;

                // Retrieve messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the source folder.");
                    return;
                }

                // Move the first message to the destination folder, preserving all properties
                ExchangeMessageInfo messageInfo = messages[0];
                client.MoveMessage(messageInfo, destinationFolderUri);

                Console.WriteLine("Message moved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
