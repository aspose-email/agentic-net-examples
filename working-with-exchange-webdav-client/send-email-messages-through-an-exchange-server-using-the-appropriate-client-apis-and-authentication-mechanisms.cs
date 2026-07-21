using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace ExchangeEmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define Exchange server connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the mail message
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("sender@example.com");
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));
                    mailMessage.Subject = "Test Email via Exchange";
                    mailMessage.Body = "This is a test email sent using Aspose.Email ExchangeClient.";

                    // Send the message using ExchangeClient
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                    {
                        client.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
