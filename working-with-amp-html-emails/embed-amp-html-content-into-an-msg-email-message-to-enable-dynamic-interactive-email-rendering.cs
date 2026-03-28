using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the output MSG file path
            string outputPath = "output/amp_email.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic message properties
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";

                // Plain‑text fallback body
                ampMessage.Body = "This is the plain text fallback for email clients that do not support AMP.";

                // HTML body for non‑AMP capable clients
                ampMessage.HtmlBody = "<h1>Hello, HTML Email!</h1>";

                // AMP HTML content
                string ampHtml = @"<!doctype html>
<html amp4email>
<head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
    <style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
    <h1>Hello, AMP Email!</h1>
</body>
</html>";
                ampMessage.AmpHtmlBody = ampHtml;

                // Save the message as an MSG file
                ampMessage.Save(outputPath);
            }

            Console.WriteLine("AMP email saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
