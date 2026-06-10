using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSmtpRetryExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP server details
                string host = "smtp.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip actual network call when placeholders are used
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP host detected. Skipping actual send.");
                    return;
                }

                // Initialize the SMTP client
                using (SmtpClient client = new SmtpClient(host, username, password))
                {
                    // Optional security configuration
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Create the email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email";
                        message.Body = "This is a test email.";

                        const int maxAttempts = 3;
                        int attempt = 0;
                        bool sent = false;

                        // Retry loop with a 15‑second interval between attempts
                        while (attempt < maxAttempts && !sent)
                        {
                            try
                            {
                                client.Send(message);
                                sent = true;
                                Console.WriteLine("Email sent successfully.");
                            }
                            catch (SmtpException ex)
                            {
                                attempt++;
                                Console.Error.WriteLine($"Send attempt {attempt} failed: {ex.Message}");
                                if (attempt < maxAttempts)
                                {
                                    Console.WriteLine("Waiting 15 seconds before retry...");
                                    Thread.Sleep(15000);
                                }
                                else
                                {
                                    Console.Error.WriteLine("All retry attempts exhausted.");
                                }
                            }
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
}
