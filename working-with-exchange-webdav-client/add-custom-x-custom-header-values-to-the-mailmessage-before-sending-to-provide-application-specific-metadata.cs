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
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "username";
            string password = "password";

            // Skip execution when placeholders are detected
            if (exchangeUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create and configure the Exchange client
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, new NetworkCredential(userName, password)))
            {
                // Build the mail message
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is the body of the email."
                );

                // Add custom X‑Custom‑Header values
                message.Headers.Add("X-Custom-Header-1", "Value1");
                message.Headers.Add("X-Custom-Header-2", "Value2");

                try
                {
                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
                finally
                {
                    // Dispose the message explicitly
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
