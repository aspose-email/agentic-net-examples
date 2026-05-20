using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values or keep placeholders)
            string host = "smtp.example.com";
            int port = 25;
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials to avoid real network calls
            bool usePlaceholders = host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                                   username.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                                   password.Equals("password", StringComparison.Ordinal);

            // Prepare a list of messages to send
            List<MailMessage> messages = new List<MailMessage>();
            for (int i = 1; i <= 5; i++)
            {
                MailMessage msg = new MailMessage
                {
                    From = username,
                    To = "recipient@example.com",
                    Subject = $"Test Message {i}",
                    Body = $"This is the body of test message {i}."
                };
                messages.Add(msg);
            }

            if (usePlaceholders)
            {
                // Simulate sending without making network calls
                foreach (var message in messages)
                {
                    Console.WriteLine($"[SIMULATED] Message '{message.Subject}' would be sent here.");
                }
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                client.Timeout = 10000; // 10 seconds

                foreach (MailMessage message in messages)
                {
                    bool sent = false;
                    while (!sent)
                    {
                        try
                        {
                            client.Send(message);
                            Console.WriteLine($"Message '{message.Subject}' sent successfully.");
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            // Check if the server responded with 421 (service not available)
                            if (ex.Message.Contains("421"))
                            {
                                Console.Error.WriteLine("Server returned 421. Pausing before retry...");
                                Thread.Sleep(TimeSpan.FromSeconds(30));
                                // Loop will retry sending the same message
                            }
                            else
                            {
                                Console.Error.WriteLine($"Failed to send message '{message.Subject}': {ex.Message}");
                                // Abort sending remaining messages on other errors
                                return;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
