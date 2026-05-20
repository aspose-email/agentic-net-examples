using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP settings detection
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send.");
                    return;
                }

                // Create the mail message
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = "sender@domain.com";
                    mailMessage.To.Add("recipient@domain.com");
                    mailMessage.Subject = "Test with custom MIME boundary";
                    mailMessage.Body = "This is the body of the email.";

                    // Define custom MIME boundary template
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                    {
                        BoundariesTemplate = "boundary--{#}-{guid}"
                    };

                    // Save to a memory stream with the custom boundary
                    using (MemoryStream ms = new MemoryStream())
                    {
                        mailMessage.Save(ms, saveOptions);
                        ms.Position = 0;

                        // Load the message back preserving the custom boundary
                        using (MailMessage messageWithCustomBoundary = MailMessage.Load(ms))
                        {
                            // Send the message via SMTP
                            using (SmtpClient client = new SmtpClient(host, port, username, password))
                            {
                                try
                                {
                                    client.Send(messageWithCustomBoundary);
                                    Console.WriteLine("Message sent successfully.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                                    return;
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
