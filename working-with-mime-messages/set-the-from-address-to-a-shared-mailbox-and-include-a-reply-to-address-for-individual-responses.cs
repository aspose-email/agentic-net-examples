using Aspose.Email.Clients.Exchange.Dav;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;


class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and host – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "sharedmailbox@example.com";
            string password = "password";

            // Guard against placeholder values to avoid real network calls during CI.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Compose the mail message.
                MailMessage message = new MailMessage();

                // Set the From address to the shared mailbox.
                message.From = new MailAddress("sharedmailbox@example.com", "Shared Mailbox");

                // Add a Reply-To address for individual responses.
                message.ReplyToList.Add(new MailAddress("user@example.com", "Individual User"));

                // Add a recipient.
                message.To.Add(new MailAddress("recipient@example.com"));

                // Set subject and body.
                message.Subject = "Test message from shared mailbox";
                message.Body = "This email is sent from a shared mailbox with a custom Reply-To address.";

                // Send the message.
                client.Send(message);
                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
