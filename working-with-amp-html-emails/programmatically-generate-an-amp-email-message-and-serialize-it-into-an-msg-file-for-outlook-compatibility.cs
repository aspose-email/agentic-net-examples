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
            string outputPath = "amp_message.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create and configure the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";

                // Set the AMP HTML body (AMP component)
                ampMessage.AmpHtmlBody = @"
<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
  <h1>Hello from AMP Email!</h1>
  <p>This is an AMP-enabled email.</p>
</body>
</html>";

                // Optionally set a fallback HTML body
                ampMessage.HtmlBody = "<p>This is a fallback HTML body for non‑AMP clients.</p>";

                // Save the message as an Outlook MSG file
                ampMessage.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
            }

            Console.WriteLine("AMP message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
