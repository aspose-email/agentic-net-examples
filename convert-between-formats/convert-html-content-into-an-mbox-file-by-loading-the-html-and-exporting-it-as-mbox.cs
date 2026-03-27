using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "input.html";
            string mboxPath = "output.mbox";

            // Verify input HTML file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Input file not found: {htmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(mboxPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
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

            // Create a mail message with the HTML body
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Converted HTML Message";
                message.HtmlBody = htmlContent;

                // Write the message to an MBOX file
                MboxSaveOptions saveOptions = new MboxSaveOptions();
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, saveOptions))
                {
                    try
                    {
                        writer.WriteMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write MBOX: {ex.Message}");
                        return;
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
