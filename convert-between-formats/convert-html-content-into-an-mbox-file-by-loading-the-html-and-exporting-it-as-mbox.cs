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
            string htmlFilePath = "sample.html";
            string outputMboxPath = "output.mbox";

            // Ensure input HTML exists; create minimal placeholder if missing
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMboxPath);
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

            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create a MailMessage with HTML body
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "HTML to MBOX Example";
            message.HtmlBody = htmlContent;

            // Write the message to an MBOX file using MboxrdStorageWriter
            try
            {
                using (FileStream fs = new FileStream(outputMboxPath, FileMode.Create, FileAccess.Write))
                {
                    MboxSaveOptions saveOptions = new MboxSaveOptions();
                    using (MboxrdStorageWriter writer = new MboxrdStorageWriter(fs, saveOptions))
                    {
                        writer.WriteMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write MBOX file: {ex.Message}");
                return;
            }

            Console.WriteLine("MBOX file created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}