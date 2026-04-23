using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://example.com/exchange/username";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("example.com") ||
                username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("sender@example.com");
                        message.To.Add(new MailAddress("recipient@example.com"));
                        message.Subject = "Test Subject";
                        message.Body = "This is a plain text body.";

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
