using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mhtml";

            // Ensure the input HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    const string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                    File.WriteAllText(inputPath, placeholderHtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document with HtmlLoadOptions.
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML as MailMessage: {ex.Message}");
                return;
            }

            // Save the message directly to MHTML format.
            using (mailMessage)
            {
                try
                {
                    mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
