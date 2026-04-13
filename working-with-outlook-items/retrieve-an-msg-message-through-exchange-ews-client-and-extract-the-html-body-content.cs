using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string messageUri = "https://exchange.example.com/EWS/MessageId";

            // Skip execution when placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Fetch the message
                    using (MailMessage mailMessage = client.FetchMessage(messageUri))
                    {
                        // Extract HTML body content
                        string htmlBody = mailMessage.HtmlBody;
                        Console.WriteLine("HTML Body:");
                        Console.WriteLine(htmlBody);
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
