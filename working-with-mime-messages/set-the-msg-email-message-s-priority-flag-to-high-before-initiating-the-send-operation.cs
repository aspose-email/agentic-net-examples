using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip actual send when placeholders are used
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create and configure the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com", "Sender");
                    message.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                    message.Subject = "Test Message with High Priority";
                    message.Body = "This is a test email with high priority.";

                    // Set the priority flag to High
                    message.Priority = MailPriority.High;

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
