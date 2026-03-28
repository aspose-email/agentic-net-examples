using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mht";

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

            // Load the HTML document into a MailMessage
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                // Configure custom MHTML save options
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.MhtFormatOptions = MhtFormatOptions.WriteHeader | MhtFormatOptions.WriteOutlineAttachments;
                saveOptions.CheckBodyContentEncoding = true;
                saveOptions.ExtractHTMLBodyResourcesAsAttachments = false;
                saveOptions.SaveAllHeaders = true;

                // Save the message as MHTML
                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
