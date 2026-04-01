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
            // Path to the source MSG file
            string msgPath = "input.msg";

            // Verify the input file exists
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

            // Load the MSG file into a MailMessage instance
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(msgPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }

            // Prepare additional recipients for the forward
            MailAddressCollection forwardRecipients = new MailAddressCollection();
            forwardRecipients.Add(new MailAddress("recipient1@example.com"));
            forwardRecipients.Add(new MailAddress("recipient2@example.com"));

            // SMTP client configuration (placeholders)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping actual send.");
                return;
            }

            // Create and use the SMTP client
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Forward the loaded message preserving its original content
                    client.Forward(smtpUser, forwardRecipients, originalMessage);

                    Console.WriteLine("Message forwarded successfully.");
                }
                catch (Exception forwardEx)
                {
                    Console.Error.WriteLine($"Failed to forward message: {forwardEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
