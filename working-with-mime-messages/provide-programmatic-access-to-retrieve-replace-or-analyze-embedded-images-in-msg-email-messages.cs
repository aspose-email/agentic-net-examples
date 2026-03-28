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
            string inputPath = "sample.msg";
            string outputPath = "modified.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Iterate over attachments to find inline images
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    MapiAttachment attachment = message.Attachments[i];

                    // Check if attachment is inline and has an image extension
                    if (attachment.IsInline)
                    {
                        string extension = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                        {
                            // Replace image data with a minimal placeholder (e.g., PNG header)
                            byte[] placeholderData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
                            attachment.BinaryData = placeholderData;
                            Console.WriteLine($"Replaced image attachment: {attachment.FileName}");
                        }
                    }
                }

                // Ensure output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Save the modified MSG
                    message.Save(outputPath);
                    Console.WriteLine($"Modified message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving modified message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
