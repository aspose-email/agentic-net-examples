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
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // AMP HTML content
            string ampHtml = @"<html amp4email>
  <head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
    <style amp4email-boilerplate>body{visibility:hidden}</style>
  </head>
  <body>
    <h1>Hello, AMP Email!</h1>
    <p>This is an AMP email body.</p>
  </body>
</html>";

            // Create and configure the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.Subject = "AMP Email Example";
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.AmpHtmlBody = ampHtml;

                // Save the message as an Outlook MSG file
                ampMessage.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
            }

            Console.WriteLine($"AMP message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
