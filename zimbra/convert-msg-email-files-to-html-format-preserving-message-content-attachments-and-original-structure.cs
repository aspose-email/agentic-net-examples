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
            // Input MSG file path
            string msgPath = "sample.msg";
            // Output HTML file path
            string htmlPath = "sample.html";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(htmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {outputDir}. {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage preserving embedded message format
                MailConversionOptions conversionOptions = new MailConversionOptions
                {
                    PreserveEmbeddedMessageFormat = true
                };
                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    // Save the message as HTML
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                    mail.Save(htmlPath, htmlOptions);
                }

                // Extract and save attachments to the same directory as the HTML file
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(outputDir ?? string.Empty, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Warning: Failed to save attachment {attachment.FileName}. {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
