using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string userPassword = "password";

            // Guard against placeholder credentials to avoid live network calls during CI.
            if (exchangeUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping operation.");
                return;
            }

            // Create the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(exchangeUri, userName, userPassword))
            {
                try
                {
                    // Placeholder message URI – replace with the actual message URI.
                    string messageUri = "/mail/inbox/12345";

                    // Guard against placeholder message URI.
                    if (string.IsNullOrWhiteSpace(messageUri) || messageUri.Contains("example.com"))
                    {
                        Console.WriteLine("Placeholder message URI detected. Skipping operation.");
                        return;
                    }

                    // Mark the message as read using a PROPPATCH call.
                    client.SetReadFlag(messageUri);

                    Console.WriteLine("Message marked as read successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while setting read flag: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
