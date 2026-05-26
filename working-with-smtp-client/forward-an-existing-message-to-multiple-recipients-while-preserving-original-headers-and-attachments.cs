using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and placeholders
            string messagePath = "original.eml";
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Guard against placeholder SMTP configuration
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Ensure the source message file exists; create a minimal placeholder if missing
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To.Add("recipient@example.com");
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder email.";
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the original message
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(messagePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message from '{messagePath}': {ex.Message}");
                return;
            }

            // Prepare recipients for forwarding
            MailAddressCollection forwardRecipients = new MailAddressCollection();
            forwardRecipients.Add("first.recipient@example.com");
            forwardRecipients.Add("second.recipient@example.com");

            // Forward the message using SmtpClient
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.Username = smtpUser;
                    client.Password = smtpPass;

                    // Forward preserving original headers and attachments
                    client.Forward(smtpUser, forwardRecipients, originalMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to forward message: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose the loaded message
                originalMessage?.Dispose();
            }

            Console.WriteLine("Message forwarded successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
