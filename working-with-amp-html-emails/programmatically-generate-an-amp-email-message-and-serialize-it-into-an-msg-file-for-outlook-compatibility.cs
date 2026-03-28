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
            // Define output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "AmpEmail.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set standard properties
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text fallback body.";

                // Set AMP HTML body
                ampMessage.AmpHtmlBody = @"
<!doctype html>
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
  <h1>Hello from AMP Email!</h1>
  <p>This content is displayed in AMP‑supported email clients.</p>
</body>
</html>";

                // Save as Outlook MSG file
                try
                {
                    ampMessage.Save(outputPath);
                    Console.WriteLine($"AMP message saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
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
