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
            // Define output MSG file path
            string outputPath = "AmpMessage_out.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic properties
                ampMessage.From = "sender@example.com";
                ampMessage.To = "recipient@example.com";
                ampMessage.Subject = "AMP Email Example";

                // Define AMP HTML content
                string ampHtml = @"<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
  <style amp-custom>
    h1 {color: #1e88e5;}
  </style>
</head>
<body>
  <h1>Hello, AMP Email!</h1>
  <p>This is an AMP-enabled email.</p>
</body>
</html>";

                // Assign AMP HTML body (optional, shown for completeness)
                ampMessage.AmpHtmlBody = ampHtml;

                // Create an AlternateView with the required MIME type
                ContentType ampContentType = new ContentType("text/x-amp-html");
                AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampHtml, ampContentType);

                // Add the AlternateView to the message
                ampMessage.AlternateViews.Add(ampView);

                // Save the message as MSG
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
