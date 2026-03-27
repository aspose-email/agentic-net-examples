using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output directory and file path
            string outputDirectory = "Output";
            string msgFilePath = Path.Combine(outputDirectory, "amp_message.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain‑text fallback.";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<html><body><h1>Hello</h1></body></html>";
                // Sample AMP HTML body (replace with actual AMP content)
                ampMessage.AmpHtmlBody = "<amp-email></amp-email>";

                // Save the message as an MSG file
                ampMessage.Save(msgFilePath);
            }

            // Create SMTP client with safety handling
            SmtpClient smtpClient;
            try
            {
                smtpClient = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create SMTP client: {ex.Message}");
                return;
            }

            using (smtpClient)
            {
                // Load the saved MSG file and send it
                try
                {
                    using (MailMessage messageToSend = MailMessage.Load(msgFilePath))
                    {
                        smtpClient.Send(messageToSend);
                        Console.WriteLine("AMP message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
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
