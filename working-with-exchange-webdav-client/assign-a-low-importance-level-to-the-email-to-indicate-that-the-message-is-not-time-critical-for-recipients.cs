using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To = "receiver@example.com";
            message.Subject = "Low importance email";
            message.Body = "This email is marked as low importance.";

            // Assign low importance using the MailPriority enumeration
            message.Priority = MailPriority.Low;

            // Placeholder Exchange server details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user";
            string password = "password";

            // Skip sending when placeholder credentials are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Connect to Exchange using WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
