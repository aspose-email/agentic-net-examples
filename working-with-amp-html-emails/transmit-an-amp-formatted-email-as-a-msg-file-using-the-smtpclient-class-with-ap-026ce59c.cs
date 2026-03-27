using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace AmpEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string outputPath = "amp_message.msg";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create the AMP message
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.From = new MailAddress("sender@example.com");
                    ampMessage.To.Add(new MailAddress("recipient@example.com"));
                    ampMessage.Subject = "AMP Email Example";
                    ampMessage.Body = "This is a plain text fallback.";
                    ampMessage.IsBodyHtml = true;
                    ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                    // Save the message as MSG
                    try
                    {
                        ampMessage.Save(outputPath);
                        Console.WriteLine($"AMP message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        return;
                    }

                    // Send the message via SMTP
                    try
                    {
                        using (SmtpClient smtpClient = new SmtpClient(
                            "smtp.example.com",
                            587,
                            "username",
                            "password",
                            SecurityOptions.Auto))
                        {
                            smtpClient.Send(ampMessage);
                            Console.WriteLine("Message sent successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
}