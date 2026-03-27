using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // SMTP server configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Output MSG file path
            string msgFilePath = "amp_email.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(msgFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create an AMP‑formatted message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress(username);
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is a fallback plain‑text body.";
                ampMessage.IsBodyHtml = false;

                // Set AMP HTML body (simple example)
                ampMessage.AmpHtmlBody = @"<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
  <h1>Hello from AMP Email!</h1>
  <p>This is an AMP‑enabled email.</p>
</body>
</html>";

                // Save the message as MSG file
                try
                {
                    ampMessage.Save(msgFilePath);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ioEx.Message}");
                    return;
                }

                // Send the message via SMTP
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                    {
                        client.Send(ampMessage);
                    }
                }
                catch (Exception smtpEx)
                {
                    Console.Error.WriteLine($"SMTP send failed: {smtpEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
