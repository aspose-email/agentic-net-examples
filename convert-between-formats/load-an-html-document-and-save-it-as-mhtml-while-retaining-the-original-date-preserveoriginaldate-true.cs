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
            string mhtmlPath = "output.mht";

            // Ensure the HTML input file exists
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

            // Create a mail message with the HTML body
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.Subject = "Converted HTML to MHTML";
                mail.HtmlBody = htmlContent;

                // Configure MHTML save options to preserve the original date
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    PreserveOriginalDate = true
                };

                try
                {
                    mail.Save(mhtmlPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
