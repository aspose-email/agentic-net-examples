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
            // Input HTML file and output MBOX file paths
            string htmlPath = "input.html";
            string mboxPath = "output.mbox";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
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

            // Create a mail message from the HTML content
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Converted HTML Message";
                message.HtmlBody = htmlContent;
                message.IsBodyHtml = true;

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(mboxPath);
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

                // Write the message to an MBOX file
                try
                {
                    MboxSaveOptions saveOptions = new MboxSaveOptions();
                    using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, saveOptions))
                    {
                        writer.WriteMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write MBOX file: {ex.Message}");
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
