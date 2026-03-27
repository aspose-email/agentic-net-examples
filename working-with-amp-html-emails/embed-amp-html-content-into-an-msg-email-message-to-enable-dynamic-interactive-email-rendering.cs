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
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "AmpMessageOutput.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic email properties
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";

                // Set AMP HTML body
                ampMessage.AmpHtmlBody = @"
<amp-html>
  <head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
    <style amp4email-boilerplate>body{visibility:hidden}</style>
  </head>
  <body>
    <h1>Hello from AMP Email!</h1>
    <p>This is an interactive AMP component.</p>
  </body>
</amp-html>";

                // Save the message as MSG file
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
