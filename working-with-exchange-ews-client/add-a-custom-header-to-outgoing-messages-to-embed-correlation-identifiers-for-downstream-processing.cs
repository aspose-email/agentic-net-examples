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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a mail message
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Test Message",
                    "This is a test email with a custom correlation header.");

                // Add custom header for correlation identifier
                string correlationId = Guid.NewGuid().ToString();
                message.Headers.Add("X-Correlation-ID", correlationId);

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent successfully with Correlation ID: " + correlationId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
