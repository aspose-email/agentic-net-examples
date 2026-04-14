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
            // Input MSG file path
            string msgPath = "input.msg";

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

            // Load the MSG file as a MailMessage (preserves attachments)
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // SMTP server configuration (placeholders)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "username";
            string smtpPassword = "password";

            // Skip real network call when placeholders are used
            if (smtpHost.Contains("example.com") ||
                smtpUsername.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                smtpPassword.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder SMTP credentials detected. Skipping forward operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                try
                {
                    client.Username = smtpUsername;
                    client.Password = smtpPassword;
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Determine sender and new recipient
                    string senderAddress = message.From != null ? message.From.Address : smtpUsername;
                    string recipientAddress = "recipient@example.com";

                    // Forward the message (attachments are retained automatically)
                    client.Forward(senderAddress, recipientAddress, message);
                    Console.WriteLine("Message forwarded successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
                    return;
                }
            }

            // Dispose the loaded message
            message.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
