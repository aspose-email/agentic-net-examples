using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output MSG file path
            string outputPath = "amp_email.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create the AMP HTML content
            string ampHtml = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello AMP</h1></body></html>";

            // Build the AMP message
            using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
            {
                // Set basic properties
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "Test AMP Email";
                ampMessage.Body = "This is a fallback plain text body.";

                // Embed the AMP HTML
                ampMessage.AmpHtmlBody = ampHtml;

                // Save the message as an MSG file
                ampMessage.Save(outputPath);
            }

            Console.WriteLine("AMP message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}