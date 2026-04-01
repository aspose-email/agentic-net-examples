using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputMhtmlPath = "output.mhtml";

            // Verify that the input HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    File.WriteAllText(inputHtmlPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputMhtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document into a MailMessage using HtmlLoadOptions.
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage message = MailMessage.Load(inputHtmlPath, loadOptions))
            {
                // Export the MailMessage directly to MHTML format, preserving content fidelity.
                message.Save(outputMhtmlPath, SaveOptions.DefaultMhtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
