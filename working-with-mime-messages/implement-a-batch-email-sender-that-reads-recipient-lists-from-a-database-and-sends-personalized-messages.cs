using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (smtpHost.Contains("example.com") || smtpUsername.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP settings detected. Skipping email sending.");
                return;
            }

            // In‑memory mock database of recipients
            List<Recipient> recipients = new List<Recipient>
            {
                new Recipient { Email = "alice@example.com", Name = "Alice" },
                new Recipient { Email = "bob@example.com", Name = "Bob" },
                new Recipient { Email = "carol@example.com", Name = "Carol" }
            };

            // Create SMTP client
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine($"SMTP credential validation failed: {credEx.Message}");
                    return;
                }

                // Prepare a collection of personalized messages
                List<MailMessage> messages = new List<MailMessage>();
                foreach (Recipient recipient in recipients)
                {
                    // Create a new mail message for each recipient
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(smtpUsername, "Sender Name");
                    message.To.Add(new MailAddress(recipient.Email, recipient.Name));
                    message.Subject = $"Hello {recipient.Name}, important update";
                    message.Body = $"Dear {recipient.Name},\n\nThis is a personalized message sent via Aspose.Email.\n\nBest regards,\nSender";

                    messages.Add(message);
                }

                // Send all messages in a batch
                try
                {
                    client.Send(messages);
                    Console.WriteLine("All emails sent successfully.");
                }
                catch (Exception sendEx)
                {
                    Console.Error.WriteLine($"Error sending emails: {sendEx.Message}");
                }
                finally
                {
                    // Dispose each MailMessage explicitly
                    foreach (MailMessage msg in messages)
                    {
                        msg.Dispose();
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

// Simple recipient model for the mock database
class Recipient
{
    public string Email { get; set; }
    public string Name { get; set; }
}
