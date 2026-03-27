using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text body.";
                ampMessage.IsBodyHtml = true;
                ampMessage.AmpHtmlBody = "<amp-email></amp-email>";

                // Save the message as an MSG file
                try
                {
                    ampMessage.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }

                // Send the message via SMTP
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                    {
                        client.Send(ampMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
