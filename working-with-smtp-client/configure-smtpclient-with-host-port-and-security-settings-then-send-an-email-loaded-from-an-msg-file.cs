using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "email.msg";

                // Verify that the MSG file exists
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                    return;
                }

                // Load the message from the MSG file
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(msgFilePath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // SMTP configuration (placeholders)
                    string smtpHost = "smtp.example.com";
                    int smtpPort = 587;
                    string smtpUsername = "user@example.com";
                    string smtpPassword = "password";

                    // Guard against placeholder credentials to avoid real network calls
                    if (smtpHost.Contains("example.com"))
                    {
                        Console.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                        return;
                    }

                    // Create and configure the SmtpClient
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword, SecurityOptions.Auto))
                    {
                        try
                        {
                            client.Send(mailMessage);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Failed to send email: {sendEx.Message}");
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
