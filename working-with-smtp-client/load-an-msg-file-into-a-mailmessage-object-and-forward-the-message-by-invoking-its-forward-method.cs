using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            const string msgPath = "input.msg";

            // Verify the MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage object
            MailMessage mailMessage = MailMessage.Load(msgPath);

            // SMTP server configuration (replace with real credentials)
            const string smtpHost = "smtp.example.com";
            const int smtpPort = 587;
            const string smtpUser = "username@example.com";
            const string smtpPassword = "password";
            const string sender = "sender@example.com";
            const string recipient = "recipient@example.com";

            // Create and use the SmtpClient to forward the message
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword, SecurityOptions.Auto))
            {
                try
                {
                    smtpClient.Forward(sender, recipient, mailMessage);
                    Console.WriteLine("Message forwarded successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to forward message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
