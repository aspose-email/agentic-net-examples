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
            // Placeholder connection data – replace with real values for actual use
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // If placeholders are detected, skip the network call to avoid runtime failures
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server connection.");
                return;
            }

            // Create and dispose the Exchange client safely
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Retrieve messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                // Filter messages whose subject contains the word "Test" (case‑insensitive)
                foreach (var msgInfo in messages)
                {
                    if (!string.IsNullOrEmpty(msgInfo.Subject) &&
                        msgInfo.Subject.IndexOf("Test", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"Filtered message: {msgInfo.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
