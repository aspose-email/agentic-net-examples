using System;
using System.IO;
using System.IO.Compression;
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

            // Directory to hold extracted attachments temporarily
            string attachmentsDir = "Attachments";

            // Output ZIP archive path
            string zipPath = "attachments.zip";

            // Ensure the attachments directory exists
            if (!Directory.Exists(attachmentsDir))
            {
                Directory.CreateDirectory(attachmentsDir);
            }

            // Verify the MSG file exists
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

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Create or overwrite the ZIP archive
                using (FileStream zipStream = new FileStream(zipPath, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                {
                    // Extract each attachment and add it to the ZIP
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        // Save attachment to a temporary file
                        string tempFilePath = Path.Combine(attachmentsDir, attachment.FileName);
                        attachment.Save(tempFilePath);

                        // Add the temporary file to the ZIP archive
                        ZipArchiveEntry entry = archive.CreateEntry(attachment.FileName, CompressionLevel.Optimal);
                        using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                        using (Stream entryStream = entry.Open())
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }

            Console.WriteLine($"All attachments have been saved to '{zipPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
