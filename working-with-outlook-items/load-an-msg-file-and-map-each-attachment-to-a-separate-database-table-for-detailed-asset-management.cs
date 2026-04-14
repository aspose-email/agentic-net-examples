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
            string msgPath = "input.msg";
            string outputDir = "Attachments";

            // Verify input MSG file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Process each attachment
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Determine a safe file name for the attachment
                    string safeFileName = Path.GetFileName(attachment.FileName);
                    if (string.IsNullOrEmpty(safeFileName))
                    {
                        safeFileName = "attachment.bin";
                    }

                    string attachmentPath = Path.Combine(outputDir, safeFileName);

                    // Save the attachment to disk
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachmentPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {ex.Message}");
                        continue;
                    }

                    // TODO: Insert attachment metadata into a database table
                    // Example: InsertAttachmentMetadata(attachment.FileName, attachmentPath, attachment.ContentId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
