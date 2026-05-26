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
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Detect placeholder credentials and skip actual network call
            if (mailboxUri.Contains("example") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create a simple mail message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Message with Custom Header";
            message.Body = "This email contains a custom X-Feedback-Id header.";

            // Add custom X-Feedback-Id header
            message.Headers.Add("X-Feedback-Id", "12345-abcde");

            // Send the message using ExchangeClient
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
