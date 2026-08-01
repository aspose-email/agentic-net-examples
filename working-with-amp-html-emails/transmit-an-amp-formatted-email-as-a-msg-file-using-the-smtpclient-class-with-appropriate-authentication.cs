using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip external operations
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password, SecurityOptions.Auto))
            {
                // Build an AMP email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress(username);
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "AMP Email Example";

                // Fallback plain text body
                message.Body = "This is a fallback plain text body.";

                // AMP HTML body (includes <amp4email> wrapper)
                string ampHtml = @"
<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
  <style amp-custom>
    .carousel-img { width:100%; height:auto; }
  </style>
</head>
<body>
  <p>This is a fallback HTML body.</p>
  <amp-carousel width=""400"" height=""200"" layout=""responsive"" type=""slides"">
    <amp-img src=""https://example.com/image1.jpg"" width=""400"" height=""200"" class=""carousel-img""></amp-img>
    <amp-img src=""https://example.com/image2.jpg"" width=""400"" height=""200"" class=""carousel-img""></amp-img>
  </amp-carousel>
</body>
</html>";
                message.HtmlBody = ampHtml;

                // Save the message as a MSG file
                string outputPath = "amp_email.msg";
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }

                // Send the AMP email via SMTP
                try
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
