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
            // Define connection parameters (placeholders)
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Create the mail message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email with Custom Header";
            message.Body = "This email includes a custom X-Release-Version header.";

            // Add custom header for traceability
            message.Headers.Add("X-Release-Version", "1.0.0");

            // Send the message using ExchangeClient
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
