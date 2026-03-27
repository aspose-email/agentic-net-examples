using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output MSG file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "AmpMessageOutput.msg");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message and set its properties
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text fallback body.";
                ampMessage.IsBodyHtml = true;

                // AMP HTML content
                string ampHtml = @"
<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
  <style amp-custom>
    h1 {color: #1a73e8;}
  </style>
</head>
<body>
  <h1>Hello from AMP Email!</h1>
  <p>This is an AMP component inside the email.</p>
</body>
</html>";
                ampMessage.AmpHtmlBody = ampHtml;

                // Convert the AMP message to a MAPI message and save as MSG
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(ampMessage))
                {
                    mapiMessage.Save(outputPath);
                }
            }

            Console.WriteLine($"AMP message saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
