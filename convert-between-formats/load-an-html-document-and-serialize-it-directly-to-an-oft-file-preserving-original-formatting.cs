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
            string htmlPath = "input.html";
            string oftPath = "output.oft";

            // Ensure input HTML exists; create a minimal placeholder if missing.
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists.
            string outputDir = Path.GetDirectoryName(oftPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load HTML content.
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

            // Create a MailMessage with the HTML body.
            using (MailMessage mail = new MailMessage())
            {
                mail.HtmlBody = htmlContent;
                mail.Subject = "Converted from HTML";

                // Convert MailMessage to MapiMessage.
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail))
                {
                    // Save as Outlook Template (OFT).
                    try
                    {
                        mapiMessage.SaveAsTemplate(oftPath);
                        Console.WriteLine($"OFT file saved to: {oftPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save OFT file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
