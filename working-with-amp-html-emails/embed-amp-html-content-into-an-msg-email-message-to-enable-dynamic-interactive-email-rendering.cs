using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output MSG file path
            string outputPath = "amp_email.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic properties
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";

                // AMP HTML content
                string ampHtml = @"<!doctype html>
<html amp4email>
<head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
    <style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
    <h1>Hello AMP Email!</h1>
    <amp-img src=""https://example.com/image.jpg"" width=""600"" height=""400"" layout=""responsive""></amp-img>
</body>
</html>";

                // Assign AMP body and a fallback HTML body
                ampMessage.AmpHtmlBody = ampHtml;
                ampMessage.HtmlBody = "<p>This is a fallback HTML version for non‑AMP clients.</p>";

                // Save the message as an Outlook MSG file
                ampMessage.Save(outputPath);
            }

            Console.WriteLine($"AMP message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
