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
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email with Correlation Header";
                message.Body = "This email contains a custom X-Correlation-Id header.";

                // Add custom correlation header
                message.Headers.Add("X-Correlation-Id", "12345");

                // Initialize Exchange client and send the message
                try
                {
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
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
