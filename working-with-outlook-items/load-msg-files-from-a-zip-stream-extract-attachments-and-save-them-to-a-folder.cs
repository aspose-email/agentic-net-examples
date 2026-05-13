using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for the zip archive and the output folder
                string zipFilePath = "messages.zip";
                string outputDirectory = "ExtractedAttachments";

                // Verify that the zip file exists
                if (!File.Exists(zipFilePath))
                {
                try
                {
                    using (FileStream zipCreate = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write))
                    using (ZipArchive zip = new ZipArchive(zipCreate, ZipArchiveMode.Create))
                    {
                        ZipArchiveEntry dummyEntry = zip.CreateEntry("placeholder.msg");
                        using (Stream entryStream = dummyEntry.Open())
                        using (MapiMessage placeholder = new MapiMessage(
                            "from@example.com",
                            "to@example.com",
                            "Placeholder Subject",
                            "Placeholder body."))
                        {
                            placeholder.Save(entryStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder ZIP: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Zip file not found: {zipFilePath}");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Open the zip archive for reading
                using (FileStream zipFileStream = File.OpenRead(zipFilePath))
                {
                    using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            // Process only MSG files
                            if (!entry.FullName.EndsWith(".msg", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            try
                            {
                                using (Stream entryStream = entry.Open())
                                {
                                    // Load the MSG file into a MapiMessage
                                    using (MapiMessage message = MapiMessage.Load(entryStream))
                                    {
                                        foreach (MapiAttachment attachment in message.Attachments)
                                        {
                                            string attachmentPath = Path.Combine(outputDirectory, attachment.FileName);
                                            try
                                            {
                                                attachment.Save(attachmentPath);
                                                Console.WriteLine($"Saved attachment: {attachmentPath}");
                                            }
                                            catch (Exception attEx)
                                            {
                                                Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {attEx.Message}");
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception msgEx)
                            {
                                Console.Error.WriteLine($"Failed to process entry '{entry.FullName}': {msgEx.Message}");
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
}
