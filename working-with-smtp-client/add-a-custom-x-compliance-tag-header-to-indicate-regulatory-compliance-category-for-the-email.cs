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
            // Define output file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "output.eml");
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Test email with compliance tag";
                message.Body = "This is a test email.";

                // Add custom X‑Compliance‑Tag header
                message.Headers.Add("X-Compliance-Tag", "Confidential");

                // Save the message to a file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }

                // Prepare SMTP client (placeholder values)
                string smtpHost = "smtp.example.com";
                if (smtpHost.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder SMTP host detected; skipping send operation.");
                }
                else
                {
                    // Instantiate the client
                    using (SmtpClient client = new SmtpClient(smtpHost, 587))
                    {
                        client.Username = "username";
                        client.Password = "password";
                        client.SecurityOptions = SecurityOptions.Auto;

                        // Send the message
                        try
                        {
                            client.Send(message);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
