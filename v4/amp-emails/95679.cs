using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        // Define output file path
        string outputPath = "amp_viewport_fittext.eml";

        // Ensure the output directory exists
        try
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        try
        {
            // Create an AMP message
            AmpMessage ampMessage = new AmpMessage();

            // Set the HTML body with a meta viewport tag
            ampMessage.HtmlBody = @"<!doctype html>
<html amp4email>
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width,minimum-scale=1,initial-scale=1"">
<style amp4email-boilerplate>body{visibility:hidden}</style>
</head>
<body>
<p>Hello, this is an AMP email with fit‑text component.</p>
</body>
</html>";

            // Add an AmpFitText component
            AmpFitText fitText = new AmpFitText("Sample fit‑text content");
            ampMessage.AddAmpComponent(fitText);

            // Save the message to an .eml file
            ampMessage.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while creating or saving the AMP message: {ex.Message}");
            return;
        }
    }
}
