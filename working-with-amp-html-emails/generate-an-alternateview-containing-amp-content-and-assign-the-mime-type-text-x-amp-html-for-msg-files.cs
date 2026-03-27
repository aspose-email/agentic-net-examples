using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "AmpMessage_out.msg";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To = "recipient@example.com";
                ampMessage.Subject = "AMP Email Example";

                // Set the AMP HTML body (optional, can also be left empty)
                ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                // Create an AlternateView with MIME type "text/x-amp-html"
                string ampContent = "<amp-html><h1>Hello AMP</h1></amp-html>";
                ContentType ampContentType = new ContentType("text/x-amp-html");
                using (AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampContent, ampContentType))
                {
                    ampMessage.AlternateViews.Add(ampView);
                }

                // Save the message as MSG file
                try
                {
                    ampMessage.Save(outputPath);
                    Console.WriteLine($"AMP message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving message: {ex.Message}");
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
