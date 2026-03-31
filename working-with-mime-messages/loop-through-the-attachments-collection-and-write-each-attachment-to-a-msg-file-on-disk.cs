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
            string inputPath = "input.msg";

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

            // Output directory for attachments
            string outputDir = "Attachments";

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                MapiAttachmentCollection attachments = message.Attachments;

                // Iterate through each attachment and save it as a separate file
                foreach (MapiAttachment attachment in attachments)
                {
                    // Determine a safe file name
                    string fileName = attachment.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "attachment.bin";
                    }

                    string outputPath = Path.Combine(outputDir, fileName);

                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment to {outputPath}");
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
