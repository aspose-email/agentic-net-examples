using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email with Retry";
                message.Body = "This email demonstrates a retry policy with exponential backoff.";

                // Create and use the SMTP client
                using (SmtpClient client = new SmtpClient(host, port, username, password, security))
                {
                    // Connection safety guard
                    try
                    {
                        // Attempt to send the message with up to three retries
                        const int maxAttempts = 3;
                        for (int attempt = 1; attempt <= maxAttempts; attempt++)
                        {
                            try
                            {
                                client.Send(message);
                                Console.WriteLine("Message sent successfully.");
                                break; // Success, exit retry loop
                            }
                            catch (SmtpException ex)
                            {
                                Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                                if (attempt == maxAttempts)
                                {
                                    Console.Error.WriteLine("All retry attempts exhausted. Giving up.");
                                    break;
                                }

                                // Exponential backoff: 2^(attempt-1) seconds
                                int delayMilliseconds = (int)Math.Pow(2, attempt - 1) * 1000;
                                Thread.Sleep(delayMilliseconds);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP client error: {ex.Message}");
                        return;
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
