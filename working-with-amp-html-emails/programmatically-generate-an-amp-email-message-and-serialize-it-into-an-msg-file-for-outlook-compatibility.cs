using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

namespace AmpEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Output file path for the MSG file
                string outputPath = "ampEmail.msg";

                // Ensure the target directory exists
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create an AMP message
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    // Basic email fields
                    ampMessage.From = new MailAddress("sender@example.com");
                    ampMessage.To.Add(new MailAddress("recipient@example.com"));
                    ampMessage.Subject = "AMP Email Example";

                    // Fallback HTML body
                    ampMessage.HtmlBody = "<html><body><h1>Hello AMP</h1><p>This is the fallback HTML content.</p></body></html>";

                    // AMP HTML body (simple example)
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
  <h1>Hello AMP</h1>
  <p>This is an AMP component example.</p>
  <amp-fit-text width=""auto"" height=""50"">
    This is an AMP fit‑text component.
  </amp-fit-text>
</body>
</html>";

                    // Save the message to MSG format with error handling
                    try
                    {
                        ampMessage.Save(outputPath);
                        Console.WriteLine($"AMP message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save AMP message: {ex.Message}");
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
}
