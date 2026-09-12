using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string htmlPath = "input.html";
            // Output MHTML file path
            string mhtmlPath = "output.mhtml";

            // Ensure input HTML exists; create a minimal placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create a MailMessage and populate basic fields
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.Subject = "Converted HTML to MHTML";
                mail.HtmlBody = htmlContent;
                // Set a sample original date
                mail.Date = DateTime.UtcNow;

                // Configure MHTML save options with original date preservation
                MhtSaveOptions mhtOptions = new MhtSaveOptions
                {
                    PreserveOriginalDate = true
                };

                // Ensure output directory exists
                try
                {
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(mhtmlPath));
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Save the message as MHTML
                try
                {
                    mail.Save(mhtmlPath, mhtOptions);
                    Console.WriteLine($"MHTML file saved successfully to '{mhtmlPath}'.");
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
