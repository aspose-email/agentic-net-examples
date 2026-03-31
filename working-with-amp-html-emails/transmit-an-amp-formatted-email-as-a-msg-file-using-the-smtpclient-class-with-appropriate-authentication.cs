using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputMsgPath = "AmpEmail.msg";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP formatted email
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text fallback.";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<p>This is the HTML fallback.</p>";
                ampMessage.AmpHtmlBody = "<amp-email><h1>AMP Content</h1></amp-email>";

                // Save the message as a MSG file
                try
                {
                    ampMessage.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ioEx.Message}");
                    return;
                }

                // SMTP client configuration (placeholder values)
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUsername = "username";
                string smtpPassword = "password";

                // Guard against placeholder credentials
                bool isPlaceholder = smtpHost.Contains("example") ||
                                      smtpUsername.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                      smtpPassword.Equals("password", StringComparison.OrdinalIgnoreCase);

                if (isPlaceholder)
                {
                    Console.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                    return;
                }

                // Send the AMP message via SMTP
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword, SecurityOptions.Auto))
                {
                    try
                    {
                        smtpClient.Send(ampMessage);
                        Console.WriteLine("AMP email sent successfully.");
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
