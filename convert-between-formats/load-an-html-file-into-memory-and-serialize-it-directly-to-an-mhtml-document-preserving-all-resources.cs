using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlFilePath = "input.html";
            string mhtmlFilePath = "output.mhtml";

            // Ensure the HTML input file exists
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    // Create a minimal placeholder HTML file
                    string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                    File.WriteAllText(htmlFilePath, placeholderHtml);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the HTML file into a MailMessage object
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(htmlFilePath, new HtmlLoadOptions()))
                {
                    // Configure MHTML save options to preserve resources
                    MhtSaveOptions mhtOptions = new MhtSaveOptions
                    {
                        ExtractHTMLBodyResourcesAsAttachments = true
                    };

                    // Save the message as MHTML
                    mailMessage.Save(mhtmlFilePath, mhtOptions);
                }
            }
            catch (Exception loadSaveEx)
            {
                Console.Error.WriteLine($"Error during load or save operation: {loadSaveEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
