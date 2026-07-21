using System;
using System.IO;
using Aspose.Email;

namespace AmpEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output MSG file path
                string outputPath = "amp_email.msg";

                // Ensure the directory for the output file exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Build a simple AMP HTML string
                string ampHtml = @"<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
  <h1>Hello, AMP Email!</h1>
  <p>This is an example of an AMP email saved as a MSG file.</p>
  <amp-img src=""https://example.com/image.jpg"" width=""600"" height=""400"" layout=""responsive""></amp-img>
</body>
</html>";

                // Create a MailMessage and embed the AMP HTML as the HTML body
                MailMessage mail = new MailMessage();
                mail.Subject = "AMP Email Example";
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.HtmlBody = ampHtml;

                // Save the message as a .msg file
                mail.Save(outputPath);
                Console.WriteLine($"AMP email saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
