using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.html";
            string outputPath = "template.oft";

            // Verify input file exists
            if (!File.Exists(inputPath))
{
    try
    {
        string placeholderHtml = "<html><body><p>Placeholder HTML content.</p></body></html>";
        File.WriteAllText(inputPath, placeholderHtml);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder HTML: {ex.Message}");
        return;
    }
}


            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the HTML document as a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputPath, new HtmlLoadOptions()))
            {
                // Convert MailMessage to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Save the MapiMessage as an Outlook File Template (OFT)
                    mapiMessage.SaveAsTemplate(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
