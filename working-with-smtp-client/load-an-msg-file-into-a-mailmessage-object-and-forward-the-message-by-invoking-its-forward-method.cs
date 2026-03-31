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
            // Define paths and placeholder SMTP settings
            string msgFilePath = "sample.msg";
            string smtpHost = "smtp.example.com"; // placeholder host
            int smtpPort = 25;
            string smtpUser = "user@example.com";
            string smtpPassword = "password";

            // Ensure the MSG file exists; create a minimal placeholder if missing
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

                try
                {
                    MailMessage placeholder = new MailMessage();
                    placeholder.Subject = "Placeholder Message";
                    placeholder.Body = "This is a placeholder MSG file.";
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    placeholder.Save(msgFilePath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MailMessage object
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Use explicit using to ensure disposal of the MailMessage
            using (message)
            {
                // Guard against placeholder SMTP settings to avoid real network calls
                if (smtpHost.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder SMTP settings detected. Skipping forward operation.");
                    return;
                }

                // Create and configure the SMTP client
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword))
                    {
                        // Forward the loaded message
                        string senderAddress = smtpUser;
                        string recipientAddress = "recipient@example.com";
                        client.Forward(senderAddress, recipientAddress, message);
                        Console.WriteLine("Message forwarded successfully.");
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
