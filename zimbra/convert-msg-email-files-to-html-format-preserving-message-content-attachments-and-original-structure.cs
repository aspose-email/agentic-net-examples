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
            string msgFilePath = "sample.msg";
            // Output HTML file path
            string htmlOutputPath = "sample.html";
            // Directory to save extracted attachments
            string attachmentsDirectory = "attachments";

            // Guard input file existence
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Ensure attachments directory exists
            try
            {
                Directory.CreateDirectory(attachmentsDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error: Unable to create attachments directory – {dirEx.Message}");
                return;
            }

            // Load the MSG file and convert to MailMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Save as HTML with embedded resources
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                    {
                        ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                    };

                    try
                    {
                        mailMessage.Save(htmlOutputPath, htmlOptions);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error: Unable to save HTML – {saveEx.Message}");
                        return;
                    }
                }

                // Extract and save attachments
                foreach (MapiAttachment attachment in mapiMessage.Attachments)
                {
                    string attachmentFileName = attachment.FileName;
                    if (string.IsNullOrEmpty(attachmentFileName))
                    {
                        attachmentFileName = "attachment.bin";
                    }

                    string attachmentPath = Path.Combine(attachmentsDirectory, attachmentFileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Warning: Failed to save attachment '{attachmentFileName}' – {attEx.Message}");
                        // Continue with other attachments
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
