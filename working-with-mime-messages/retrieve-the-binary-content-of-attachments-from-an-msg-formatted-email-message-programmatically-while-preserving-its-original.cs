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
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG message
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Check for attachments
                if (message.Attachments == null || message.Attachments.Count == 0)
                {
                    Console.WriteLine("No attachments found.");
                    return;
                }

                // Prepare output directory
                string outputDir = "Attachments";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Extract each attachment preserving its original binary content
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string fileName = attachment.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "UnnamedAttachment.bin";
                    }

                    string outputPath = Path.Combine(outputDir, fileName);

                    try
                    {
                        // Save the raw attachment data
                        attachment.Save(outputPath);
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
