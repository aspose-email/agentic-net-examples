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
            // Path to the source email file (EML, MSG, etc.)
            string inputPath = "input.msg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Prepare output directory
                string outputDir = "ExtractedTnefAttachments";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Iterate through all attachments
                foreach (Attachment attachment in message.Attachments)
                {
                    // Process only TNEF (winmail.dat) attachments
                    if (attachment.IsTnef)
                    {
                        // Load the TNEF attachment as a MapiMessage
                        using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(attachment.ContentStream))
                        {
                            // Build output MSG file name
                            string baseName = Path.GetFileNameWithoutExtension(attachment.Name);
                            if (string.IsNullOrEmpty(baseName))
                            {
                                baseName = "attachment";
                            }
                            string outputPath = Path.Combine(outputDir, baseName + ".msg");

                            // Save the extracted attachment as MSG
                            tnefMessage.Save(outputPath);
                            Console.WriteLine($"Saved TNEF attachment as MSG: {outputPath}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
