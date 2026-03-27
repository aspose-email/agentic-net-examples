using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

namespace AmpMessageSample
{
    class Program
    {
        static void Main()
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

                // AMP HTML content
                string ampHtml = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script><style amp4email-boilerplate>body{visibility:hidden}</style></head><body><h1>Hello AMP Email</h1></body></html>";

                // Create and configure the AMP message
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                    ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                    ampMessage.Subject = "AMP Email Example";
                    ampMessage.Body = "This is a fallback plain text body.";
                    ampMessage.IsBodyHtml = true;
                    ampMessage.AmpHtmlBody = ampHtml;

                    // Save the message as an MSG file
                    ampMessage.Save(msgFilePath);
                }

                Console.WriteLine($"AMP message saved to: {msgFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
