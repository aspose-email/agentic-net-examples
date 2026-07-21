using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the MSG file
            string msgPath = "input.msg";

            // Guard: ensure the file exists before attempting to load
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.WriteLine($"Message file '{msgPath}' not found.");
                return;
            }

            try
            {
                // Load the MSG file into a MailMessage instance
                using (MailMessage message = MailMessage.Load(msgPath))
                {
                    // Set the priority flag to High
                    message.Priority = MailPriority.High;

                    // SMTP server configuration (placeholders)
                    string smtpHost = "smtp.example.com";
                    int smtpPort = 587;
                    string smtpUser = "user@example.com";
                    string smtpPassword = "password";

                    // Guard: skip network operation if placeholder credentials are detected
                    if (smtpHost.Contains("example.com") || smtpUser.Contains("example.com"))
                    {
                        Console.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                        return;
                    }

                    // Initialize and configure the SMTP client
                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtpClient.Username = smtpUser;
                        smtpClient.Password = smtpPassword;
                        smtpClient.SecurityOptions = SecurityOptions.Auto;

                        try
                        {
                            // Send the message
                            smtpClient.Send(message);
                            Console.WriteLine("Message sent successfully.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.WriteLine($"Error sending message: {sendEx.Message}");
                        }
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.WriteLine($"Error loading message: {loadEx.Message}");
            }
        }
    }
}
