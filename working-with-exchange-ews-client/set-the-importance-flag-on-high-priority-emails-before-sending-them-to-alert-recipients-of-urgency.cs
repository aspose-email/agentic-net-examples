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
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("your.") ||
                username.Contains("your_") ||
                password.Contains("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create a high‑importance mail message.
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Urgent: Action Required";
            message.Body = "Please address this issue as soon as possible.";
            // Set the Importance header to High.
            message.Headers.Add("Importance", "High");

            // Connect to Exchange using EWS and send the message.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully with high importance.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
