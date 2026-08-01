using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Amp;

// Author: Generated example for creating an AMP message with an AlternateView and saving as MSG
class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "AmpMessage_out.msg";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To = "recipient@example.com";
                ampMessage.Subject = "AMP Email Example";

                // AMP HTML content
                string ampHtml = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello AMP</h1></body></html>";

                // Create an AlternateView with the required MIME type for AMP
                AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampHtml, Encoding.UTF8, "text/x-amp-html");
                ampMessage.AddAlternateView(ampView);

                // Optionally set the AmpHtmlBody property
                ampMessage.AmpHtmlBody = ampHtml;

                // Save the message as an MSG file
                try
                {
                    ampMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
