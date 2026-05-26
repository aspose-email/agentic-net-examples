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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/hosts
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create the mail message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Marketing Campaign";
            message.Body = "This is a test email for the marketing campaign.";
            // Add custom X-Campaign-Id header
            message.Headers.Add("X-Campaign-Id", "Campaign123");

            // Send the message using ExchangeClient
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
                Console.Error.WriteLine($"Error sending email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
