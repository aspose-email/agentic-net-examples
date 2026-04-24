using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "exchange.example.com";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid unwanted network calls.
            if (host.Contains("example.com") || username.Contains("username") || password.Contains("password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the Exchange client.
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                // Unique identifier (URI) of the message to be deleted.
                string messageUri = "https://exchange.example.com/.../MessageId";

                // URI of the Deleted Items folder.
                string deletedItemsUri = client.MailboxInfo.DeletedItemsUri;

                try
                {
                    // Move the message to Deleted Items (preferred way over DeleteMessage).
                    client.MoveMessage(deletedItemsUri, messageUri);
                    Console.WriteLine("Message successfully deleted (moved to Deleted Items).");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete the message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
