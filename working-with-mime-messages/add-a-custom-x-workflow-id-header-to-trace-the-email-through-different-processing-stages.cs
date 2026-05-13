using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls during CI
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Initialize the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email with Custom Header";
                    message.Body = "This email contains a custom X-Workflow-Id header.";

                    // Add custom X-Workflow-Id header
                    message.Headers.Add("X-Workflow-Id", "12345");

                    try
                    {
                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
