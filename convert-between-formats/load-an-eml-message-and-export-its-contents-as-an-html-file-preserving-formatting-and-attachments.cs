using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input EML file path
            string emlPath = "sample.eml";
            // Output HTML file path
            string outputHtmlPath = "output.html";
            // Directory to save attachments
            string attachmentsDirectory = "attachments";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Ensure output directories exist
            try
            {
                string htmlDirectory = Path.GetDirectoryName(outputHtmlPath);
                if (!string.IsNullOrEmpty(htmlDirectory) && !Directory.Exists(htmlDirectory))
                {
                    Directory.CreateDirectory(htmlDirectory);
                }
                if (!Directory.Exists(attachmentsDirectory))
                {
                    Directory.CreateDirectory(attachmentsDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating directories: {dirEx.Message}");
                return;
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert to MapiMessage to access HTML body and attachments
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Export HTML body
                    string bodyHtml = mapiMessage.BodyHtml ?? string.Empty;
                    try
                    {
                        using (FileStream htmlStream = new FileStream(outputHtmlPath, FileMode.Create, FileAccess.Write))
                        using (StreamWriter writer = new StreamWriter(htmlStream))
                        {
                            writer.Write(bodyHtml);
                        }
                    }
                    catch (Exception htmlEx)
                    {
                        Console.Error.WriteLine($"Error writing HTML file: {htmlEx.Message}");
                        return;
                    }

                    // Save each attachment to the attachments directory
                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        try
                        {
                            string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {attEx.Message}");
                        }
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