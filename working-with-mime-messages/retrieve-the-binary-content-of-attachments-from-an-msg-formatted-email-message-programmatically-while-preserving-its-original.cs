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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
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

                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject",
                        "Placeholder body");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = "Attachments";
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract attachments
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    byte[] binaryData = attachment.BinaryData;
                    if (binaryData == null || binaryData.Length == 0)
                    {
                        continue; // Skip empty attachments
                    }

                    string fileName = attachment.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = attachment.LongFileName;
                    }
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "attachment.bin";
                    }

                    // Sanitize file name
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        fileName = fileName.Replace(invalidChar, '_');
                    }

                    string outputPath = Path.Combine(outputDir, fileName);
                    try
                    {
                        File.WriteAllBytes(outputPath, binaryData);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{fileName}': {ex.Message}");
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
