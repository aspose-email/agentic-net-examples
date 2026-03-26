using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            // Ensure the input EML file exists
            if (!File.Exists(emlPath))
            {
                // Create a minimal placeholder EML file
                File.WriteAllText(emlPath, "From: test@example.com\r\nSubject: Sample\r\n\r\nThis is a sample email.");
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Configure HTML save options to save attachments as separate files
                HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                saveOptions.ResourceRenderingMode = ResourceRenderingMode.SaveToFile;

                string htmlPath = emlPath + ".html";

                // Save the message as HTML
                mailMessage.Save(htmlPath, saveOptions);
                Console.WriteLine("EML converted to HTML successfully: " + htmlPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}