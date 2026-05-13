using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Folder containing MSG files
            string inputFolder = "MsgFolder";
            // Output ZIP archive path
            string outputZipPath = "attachments.zip";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Create (or overwrite) the ZIP archive
            try
            {
                using (FileStream zipFileStream = new FileStream(outputZipPath, FileMode.Create, FileAccess.ReadWrite))
                using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    // Process each MSG file in the folder
                    string[] msgFiles = Directory.GetFiles(inputFolder, "*.msg");
                    foreach (string msgFilePath in msgFiles)
                    {
                        // Guard against missing files (should not happen with GetFiles)
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

                            Console.Error.WriteLine($"File not found: {msgFilePath}");
                            continue;
                        }

                        try
                        {
                            using (MapiMessage message = MapiMessage.Load(msgFilePath))
                            {
                                foreach (MapiAttachment attachment in message.Attachments)
                                {
                                    // Load attachment into memory
                                    using (MemoryStream attachmentStream = new MemoryStream())
                                    {
                                        attachment.Save(attachmentStream);
                                        attachmentStream.Position = 0;

                                        // Create an entry in the ZIP archive
                                        string entryName = attachment.FileName;
                                        ZipArchiveEntry entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                                        using (Stream entryStream = entry.Open())
                                        {
                                            attachmentStream.CopyTo(entryStream);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing '{msgFilePath}': {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"All attachments have been compressed into '{outputZipPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create ZIP archive: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
