using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace SmtpBatchSend
{
    class Program
    {
        static void Main()
        {
            try
            {
                // SMTP server configuration (replace with real values)
                string host = "smtp.example.com";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid external calls during CI
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                    return;
                }

                // Create the SMTP client and ensure it is disposed properly
                using (SmtpClient client = new SmtpClient(host, username, password))
                {
                    try
                    {
                        // Optional: validate credentials before sending
                        client.ValidateCredentials();

                        // Create a single independent connection to be reused for the batch
                        using (IConnection connection = client.CreateConnection())
                        {
                            // Prepare a batch of fifty email messages
                            List<MailMessage> messages = new List<MailMessage>();
                            for (int i = 1; i <= 50; i++)
                            {
                                MailMessage message = new MailMessage();
                                message.From = username;
                                message.To.Add("recipient@example.com");
                                message.Subject = $"Test Email {i}";
                                message.Body = $"This is the body of test email number {i}.";
                                messages.Add(message);
                            }

                            // Send all messages using the same connection
                            client.Send(connection, messages);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
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
}
