using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string sourcePath = "source.html";
            string targetPath = "target.emlx";

            // Ensure the source HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    File.WriteAllText(sourcePath, "<html><body>Hello, World!</body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string targetDir = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
            {
                try
                {
                    Directory.CreateDirectory(targetDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the HTML message with default HtmlLoadOptions
            using (MailMessage mailMessage = MailMessage.Load(sourcePath, new HtmlLoadOptions()))
            {
                // Prepare save options for EMLX format
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);

                // Save the message as EMLX
                try
                {
                    mailMessage.Save(targetPath, emlSaveOptions);
                    Console.WriteLine($"Message saved successfully to '{targetPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message as EMLX: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
