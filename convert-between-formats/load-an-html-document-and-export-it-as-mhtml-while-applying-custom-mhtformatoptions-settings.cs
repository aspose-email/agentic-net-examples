using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "sample.html";
            string mhtmlPath = "output.mht";

            // Ensure the HTML source file exists
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><h1>Placeholder</h1></body></html>");
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

            // Create a mail message and set its HTML body
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To.Add("recipient@example.com");
                mail.Subject = "HTML to MHTML conversion";
                mail.HtmlBody = htmlContent;

                // Configure MHTML save options with custom format flags
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.MhtFormatOptions = MhtFormatOptions.WriteHeader |
                                                MhtFormatOptions.WriteOutlineAttachments |
                                                MhtFormatOptions.WriteCompleteEmailAddress;

                // Save the message as MHTML
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

            Console.WriteLine("MHTML file created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
