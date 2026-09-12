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
            string outputPath = "amp_email.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP email message
            AmpMessage ampMessage = new AmpMessage();
            ampMessage.Subject = "AMP Email Example";
            ampMessage.From = new MailAddress("sender@example.com");
            ampMessage.To.Add(new MailAddress("receiver@example.com"));

            // Embed AMP HTML content.
            // Note: The exact property name may vary between Aspose.Email versions.
            // Adjust the property used to set AMP content if necessary.
            ampMessage.AmpHtmlBody = @"<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
</head>
<body>
  <p>Hello, this is an AMP-enabled email!</p>
</body>
</html>";

            // Save the message as an MSG file
            ampMessage.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
