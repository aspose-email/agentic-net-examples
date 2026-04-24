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
            // Define a custom AMP HTML template with placeholders
            string ampTemplate = @"<!doctype html>
<html amp4email>
<head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
</head>
<body>
    <h1>Hello {{FirstName}} {{LastName}}</h1>
    <p>This is an AMP email.</p>
</body>
</html>";

            // Create an AMP message and set basic properties
            using (AmpMessage message = new AmpMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "AMP Email Example";

                // Set the AMP HTML body using the template
                message.AmpHtmlBody = ampTemplate;

                // Save the message to a file (guarded file I/O)
                string outputPath = "amp_message.eml";
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    message.Save(outputPath);
                    Console.WriteLine($"AMP message saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
