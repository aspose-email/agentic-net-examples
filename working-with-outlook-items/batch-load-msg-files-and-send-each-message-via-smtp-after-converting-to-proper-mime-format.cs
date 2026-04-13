using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string inputDir = "Messages";

            // Ensure the input directory exists
            if (!Directory.Exists(inputDir))
                Directory.CreateDirectory(inputDir);

            // Create a minimal placeholder MSG if none exist
            string placeholderPath = Path.Combine(inputDir, "placeholder.msg");
            if (!File.Exists(placeholderPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder message.",
                    OutlookMessageFormat.Unicode))
                {
                    placeholder.Save(placeholderPath);
                }
            }

            string[] msgFiles = Directory.GetFiles(inputDir, "*.msg");
            foreach (string msgFile in msgFiles)
            {
                // Load each MSG file safely
                try
                {
                    using (MapiMessage mapiMsg = MapiMessage.Load(msgFile))
                    {
                        // Convert to MailMessage with proper conversion options
                        using (MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions()))
                        {
                            // Placeholder SMTP settings
                            string smtpHost = "smtp.example.com";
                            int smtpPort = 25;
                            string smtpUser = "user@example.com";
                            string smtpPass = "password";

                            // Skip real network call when using placeholder credentials/host
                            if (smtpHost.Contains("example.com"))
                            {
                                Console.WriteLine($"Skipping send for '{msgFile}' due to placeholder SMTP host.");
                                continue;
                            }

                            // Send via SMTP with connection safety
                            try
                            {
                                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                                {
                                    client.Send(mailMsg);
                                    Console.WriteLine($"Sent '{msgFile}' successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error sending '{msgFile}': {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgFile}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
