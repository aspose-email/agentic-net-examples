using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "amp_message.msg";
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain‑text fallback.";
                ampMessage.HtmlBody = "<html><body><h1>Hello</h1></body></html>";
                ampMessage.AmpHtmlBody = "<amp-html><h1>AMP Content</h1></amp-html>";

                // Send the message via SMTP
                using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        smtpClient.Send(ampMessage);
                    }
                    catch (Exception smtpEx)
                    {
                        Console.Error.WriteLine("SMTP send error: " + smtpEx.Message);
                        return;
                    }
                }

                // Save the message as a MSG file
                try
                {
                    ampMessage.Save(outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine("File save error: " + saveEx.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}