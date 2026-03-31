using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "amp_message.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To = "recipient@example.com";
                ampMessage.Subject = "AMP Email Example";

                // AMP HTML content
                string ampHtml = @"<!doctype html>
<html amp4email>
<head>
<meta charset=""utf-8"">
<script async src=""https://cdn.ampproject.org/v0.js""></script>
</head>
<body>
<h1>Hello AMP</h1>
</body>
</html>";

                // Create the appropriate content type for AMP
                ContentType ampContentType = new ContentType("text/x-amp-html");

                // Create an AlternateView with the AMP content
                using (AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampHtml, ampContentType))
                {
                    ampMessage.AlternateViews.Add(ampView);
                }

                // Save the message as an Outlook MSG file
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                ampMessage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
