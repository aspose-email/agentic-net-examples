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
            string sourceFolderPath = "MsgFolder";
            // Output ZIP archive path
            string zipFilePath = "AllAttachments.zip";

            // Verify source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Error: Folder not found – {sourceFolderPath}");
                return;
            }

            // Create or overwrite the ZIP archive
            try
            {
                using (FileStream zipFileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                    {
                        // Enumerate all MSG files in the folder
                        string[] msgFiles = Directory.GetFiles(sourceFolderPath, "*.msg");
                        foreach (string msgFilePath in msgFiles)
                        {
                            // Ensure the MSG file exists (should be true from GetFiles)
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

                                Console.Error.WriteLine($"Warning: File not found – {msgFilePath}");
                                continue;
                            }

                            try
                            {
                                // Load the MSG file
                                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                                {
                                    // Iterate through each attachment in the message
                                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                                    {
                                        // Create a unique entry name to avoid collisions
                                        string entryName = Path.GetFileNameWithoutExtension(msgFilePath) + "_" + attachment.FileName;
                                        ZipArchiveEntry zipEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
                                        using (Stream entryStream = zipEntry.Open())
                                        {
                                            // Save attachment directly into the ZIP entry stream
                                            attachment.Save(entryStream);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                                // Continue with next file
                            }
                        }
                    }
                }

                Console.WriteLine($"All attachments have been compressed into '{zipFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating ZIP archive: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
