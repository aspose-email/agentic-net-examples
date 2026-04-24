using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if they are not real.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username@example.com";
            string password = "password";

            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                mailboxUri.Contains("example") ||
                username.Contains("example") ||
                password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping Exchange operations.");
                return;
            }

            // Create the Exchange client inside a using block to ensure proper disposal.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Verify connection by attempting to list messages in the Inbox.
                ExchangeMessageInfoCollection inboxMessages;
                try
                {
                    inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Destination folder – assume it already exists.
                string destinationFolderUri = "ProjectX";

                // Iterate over each message and move it to the destination folder.
                foreach (var msgInfo in inboxMessages)
                {
                    try
                    {
                        client.MoveMessage(msgInfo, destinationFolderUri);
                        Console.WriteLine($"Moved message '{msgInfo.Subject}' to '{destinationFolderUri}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message '{msgInfo.Subject}': {ex.Message}");
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
