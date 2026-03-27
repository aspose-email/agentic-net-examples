using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string emlPath = "input.eml";
            const string htmlPath = "output.html";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    const string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message.
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Ensure the body is encoded as UTF‑8.
                mailMessage.BodyEncoding = Encoding.UTF8;

                // Configure HTML save options (no Encoding property exists).
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    // Example: embed resources directly into the HTML.
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml,
                    // Enable checking of body content encoding to enforce UTF‑8.
                    CheckBodyContentEncoding = true
                };

                // Save as HTML.
                try
                {
                    mailMessage.Save(htmlPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save HTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
