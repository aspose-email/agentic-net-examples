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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG message
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Create a ZIP archive in memory containing all original attachments
                using (MemoryStream zipStream = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            byte[] data = attachment.BinaryData;
                            if (data == null) continue; // skip empty attachments

                            ZipArchiveEntry entry = archive.CreateEntry(attachment.FileName ?? "Unnamed");
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.Write(data, 0, data.Length);
                            }
                        }
                    }

                    // Prepare ZIP data
                    zipStream.Position = 0;
                    byte[] zipBytes = zipStream.ToArray();

                    // Remove existing attachments
                    for (int i = message.Attachments.Count - 1; i >= 0; i--)
                    {
                        message.Attachments.RemoveAt(i);
                    }

                    // Add the single ZIP attachment
                    message.Attachments.Add("AllAttachments.zip", zipBytes);
                }

                // Save the modified message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with compressed attachments to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
