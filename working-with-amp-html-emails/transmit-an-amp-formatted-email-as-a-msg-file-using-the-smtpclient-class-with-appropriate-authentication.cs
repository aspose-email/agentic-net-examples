using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "amp_message.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";

                // Plain HTML body
                ampMessage.HtmlBody = "<html><body><h1>Hello</h1></body></html>";

                // AMP HTML body
                ampMessage.AmpHtmlBody = @"<!doctype html>
<html amp4email>
<head>
<meta charset=""utf-8"">
<script async src=""https://cdn.ampproject.org/v0.js""></script>
</head>
<body>
<h1>AMP Content</h1>
</body>
</html>";

                // Save the message as a MSG file
                ampMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);

                // Send the message via SMTP with authentication
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                {
                    client.Send(ampMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
