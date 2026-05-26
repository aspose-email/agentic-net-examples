using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple mail message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "Hello"))
            {
                // Set a custom tracking header
                message.Headers["X-Mailing-ID"] = "ABC-12345";

                // Placeholder connection details
                string host = "exchange.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip sending when placeholder values are detected
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder host detected. Skipping send operation.");
                    return;
                }

                // Send the message using ExchangeClient
                try
                {
                    using (ExchangeClient client = new ExchangeClient(host, username, password))
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
