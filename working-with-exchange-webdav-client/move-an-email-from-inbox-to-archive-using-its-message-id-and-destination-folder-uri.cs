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
            // Placeholder connection details
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            // Message URI to move and destination folder URI (Archive)
            string messageUri = "https://exchange.example.com/.../MessageId";
            string archiveFolderUri = "Archive";

            using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
            {
                try
                {
                    client.MoveMessage(archiveFolderUri, messageUri);
                    Console.WriteLine("Message moved to Archive successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error moving message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
