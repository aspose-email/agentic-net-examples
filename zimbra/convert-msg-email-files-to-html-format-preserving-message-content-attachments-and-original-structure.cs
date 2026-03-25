using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "sample.msg";
            // Output HTML file path
            string outputHtmlPath = "output.html";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDir}");
                    Console.Error.WriteLine(dirEx.Message);
                    return;
                }
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Convert to MailMessage preserving embedded content
                MailConversionOptions conversionOptions = new MailConversionOptions();
                conversionOptions.PreserveEmbeddedMessageFormat = true;
                conversionOptions.PreserveRtfContent = true;

                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Save as HTML, preserving attachments and structure
                    mailMessage.Save(outputHtmlPath, SaveOptions.DefaultHtml);
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}