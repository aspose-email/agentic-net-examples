using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip actual network call
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email transmission.");
                return;
            }

            // Create the mail message with high priority
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Urgent: Action Required";
            message.Body = "Please address this issue as soon as possible.";
            message.Priority = MailPriority.High; // High priority flag

            // Send the message using ExchangeClient
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully with high priority.");
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
