using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (placeholders)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping actual send operation.");
                return;
            }

            // Prepare a collection of email messages to send
            List<MailMessage> messages = new List<MailMessage>();
            for (int i = 1; i <= 5; i++)
            {
                MailMessage message = new MailMessage();
                message.From = username;
                message.To.Add(username);
                message.Subject = $"Test Email {i}";
                message.Body = $"This is the body of test email #{i}.";
                messages.Add(message);
            }

            // Create and use the SMTP client
            try
            {
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    int total = messages.Count;
                    int sentCount = 0;

                    foreach (MailMessage msg in messages)
                    {
                        try
                        {
                            client.Send(msg);
                            sentCount++;
                            Console.WriteLine($"Sent {sentCount}/{total} emails.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Failed to send email '{msg.Subject}': {sendEx.Message}");
                        }
                        finally
                        {
                            // Dispose each message after sending
                            msg.Dispose();
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"SMTP client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
