using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "input.html";
            string mhtmlPath = "output.mhtml";

            // Ensure the input HTML file exists
            if (!File.Exists(htmlPath))
            {
                try
                {
                    // Create a minimal placeholder HTML file
                    string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                    File.WriteAllText(htmlPath, placeholderHtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document as a MailMessage using HtmlLoadOptions
            try
            {
                using (MailMessage message = MailMessage.Load(htmlPath, new HtmlLoadOptions()))
                {
                    // Save the MailMessage directly to MHTML format
                    try
                    {
                        message.Save(mhtmlPath, SaveOptions.DefaultMhtml);
                        Console.WriteLine($"Successfully saved MHTML to '{mhtmlPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MHTML file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading HTML file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
