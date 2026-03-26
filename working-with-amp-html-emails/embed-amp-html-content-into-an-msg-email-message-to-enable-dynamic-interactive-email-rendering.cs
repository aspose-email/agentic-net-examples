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
            string outputPath = "AmpMessageOutput.msg";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine("Failed to create directory: " + dirEx.Message);
                    return;
                }
            }

            // Create an AMP message and set its properties
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";

                // Set plain text body (fallback for non‑AMP clients)
                ampMessage.Body = "This email contains AMP content. If you see this, your client does not support AMP.";

                // Set AMP HTML body
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
  <p>This is an interactive AMP component.</p>
</body>
</html>";

                // Save the message to a file using a stream and default MSG save options
                try
                {
                    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        ampMessage.Save(fileStream, SaveOptions.DefaultMsg);
                    }
                    Console.WriteLine("AMP message saved successfully to: " + outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine("Failed to save AMP message: " + saveEx.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}