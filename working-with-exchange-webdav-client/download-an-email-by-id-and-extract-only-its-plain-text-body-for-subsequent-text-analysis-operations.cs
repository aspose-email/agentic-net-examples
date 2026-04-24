using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";
            string messageId = "AAMk..."; // placeholder message ID

            // Skip execution when placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    MailMessage message = client.FetchMessage(messageId);
                    using (message)
                    {
                        string plainTextBody = message.Body;
                        Console.WriteLine("Plain‑text body:");
                        Console.WriteLine(plainTextBody);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching message: {ex.Message}");
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
