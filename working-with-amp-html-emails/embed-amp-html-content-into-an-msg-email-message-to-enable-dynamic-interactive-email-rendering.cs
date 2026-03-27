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

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic properties
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email Example";

                // Plain text body (fallback)
                ampMessage.Body = "This email contains AMP content. View it in a supported client.";

                // HTML body (fallback)
                ampMessage.HtmlBody = "<p>This email contains AMP content. View it in a supported client.</p>";

                // AMP HTML body
                ampMessage.AmpHtmlBody = @"
<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
  <style amp-custom>
    h1 {color: #1e88e5;}
    .button {background:#1e88e5;color:#fff;padding:10px 20px;text-decoration:none;}
  </style>
</head>
<body>
  <h1>Welcome to AMP Email!</h1>
  <p>This is an interactive AMP component.</p>
  <a class=""button"" href=""https://example.com"">Visit Site</a>
</body>
</html>";

                // Save the message to a file using a stream and default MSG options
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fs, SaveOptions.DefaultMsg);
                }

                Console.WriteLine($"AMP email saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
