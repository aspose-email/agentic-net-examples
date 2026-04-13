using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input MSG file and output HTML preview
            string inputFilePath = "sample.msg";
            string outputFilePath = "preview.html";

            // Verify that the input MSG file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file with options to preserve its content
            using (MailMessage message = MailMessage.Load(inputFilePath, new MsgLoadOptions()))
            {
                // Prefer the HTML body if available
                string htmlContent = message.HtmlBody;

                // Fallback to plain text wrapped in minimal HTML if HtmlBody is empty
                if (string.IsNullOrEmpty(htmlContent))
                {
                    string plainBody = System.Net.WebUtility.HtmlEncode(message.Body ?? string.Empty);
                    htmlContent = $"<html><body><pre>{plainBody}</pre></body></html>";
                }

                // Write the HTML preview to the output file
                try
                {
                    File.WriteAllText(outputFilePath, htmlContent);
                    Console.WriteLine($"HTML preview saved to {outputFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write HTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
