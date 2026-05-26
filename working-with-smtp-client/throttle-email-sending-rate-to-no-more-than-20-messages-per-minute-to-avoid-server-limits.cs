using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder configuration
            if (string.IsNullOrWhiteSpace(host) || host.Contains("example.com") ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("SMTP configuration is missing or contains placeholder values.");
                return;
            }

            // Prepare a batch of messages to send
            List<MailMessage> messages = new List<MailMessage>();
            for (int i = 1; i <= 10; i++)
            {
                MailMessage msg = new MailMessage();
                msg.From = username;
                msg.To.Add(username);
                msg.Subject = $"Test Message {i}";
                msg.Body = $"This is the body of test message {i}.";
                messages.Add(msg);
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                // Validate credentials safely
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine($"Failed to validate SMTP credentials: {credEx.Message}");
                    return;
                }

                // Send messages with throttling (max 20 per minute => 3 seconds interval)
                foreach (MailMessage message in messages)
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine($"Sent: {message.Subject}");
                    }
                    catch (Exception sendEx)
                    {
                        Console.Error.WriteLine($"Error sending message '{message.Subject}': {sendEx.Message}");
                    }

                    // Wait 3 seconds before sending the next message
                    Thread.Sleep(3000);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
