using System;
using System.Collections.Generic;
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

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping email send operation.");
                return;
            }

            // Prepare a list of 50 recipient email addresses
            List<string> recipients = new List<string>();
            for (int i = 1; i <= 50; i++)
            {
                recipients.Add($"recipient{i}@example.org");
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    foreach (string recipient in recipients)
                    {
                        // Create a personalized mail message
                        using (MailMessage message = new MailMessage())
                        {
                            message.From = username;
                            message.To.Add(recipient);
                            message.Subject = $"Hello Recipient {recipient}";
                            message.Body = $"Dear {recipient},\n\nThis is a personalized message sent via Aspose.Email.\n\nBest regards,\n{username}";
                            
                            // Send the message
                            client.Send(message);
                            Console.WriteLine($"Sent email to {recipient}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during sending emails: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
