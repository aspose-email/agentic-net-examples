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
            // Input MSG file path
            string msgPath = @"C:\Temp\sample.msg";
            // Output directory for extracted attachments
            string outputDir = @"C:\Temp\Attachments";

            // Verify input file exists
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
                // Optionally create a minimal placeholder MSG to continue
                try
                {
                    MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com");
                    placeholder.Save(msgPath);
                    Console.WriteLine($"Created placeholder MSG at {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Iterate over attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Determine a safe file name
                    string fileName = !string.IsNullOrEmpty(attachment.FileName)
                        ? attachment.FileName
                        : (!string.IsNullOrEmpty(attachment.LongFileName) ? attachment.LongFileName : "attachment.bin");

                    // Combine with output directory
                    string outputPath = Path.Combine(outputDir, fileName);

                    // Write attachment bytes to file
                    try
                    {
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] data = attachment.BinaryData;
                            if (data != null && data.Length > 0)
                            {
                                fileStream.Write(data, 0, data.Length);
                                Console.WriteLine($"Saved attachment: {outputPath}");
                            }
                            else
                            {
                                Console.WriteLine($"Attachment {fileName} has no data.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {fileName}: {ex.Message}");
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
