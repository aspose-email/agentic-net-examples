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
            // Define output MSG file path
            string outputPath = "output.msg";
            string outputDirectory = Path.GetDirectoryName(outputPath);

            // Ensure the output directory exists
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP message and set its properties
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";

                // AMP HTML content
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
  <p>This is an AMP-enabled email.</p>
</body>
</html>";

                // Convert the AMP message to a MAPI message
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(ampMessage))
                {
                    // Save the MAPI message as a MSG file
                    mapiMessage.Save(outputPath);
                }
            }

            Console.WriteLine("AMP message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}