using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "sample.msg";

            // Directory where extracted images will be saved
            string outputDirectory = "ExtractedImages";

            // Verify input file exists; if not, create a placeholder MSG (optional)
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (var placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Load the MSG file
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Iterate over all attachments in the MSG
            foreach (MapiAttachment attachment in mapMsg.Attachments)
            {
                // Determine if the attachment is an image based on file extension
                string fileName = attachment.FileName ?? string.Empty;
                bool isImage = fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                               fileName.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase);

                if (!isImage)
                    continue;

                // Create a safe file name
                string safeFileName = string.IsNullOrWhiteSpace(fileName) ? "image" : fileName;
                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    safeFileName = safeFileName.Replace(invalidChar, '_');

                string outputPath = Path.Combine(outputDirectory, safeFileName);

                // Save the image preserving its original format and quality using BinaryData
                try
                {
                    byte[] data = attachment.BinaryData;
                    if (data == null || data.Length == 0)
                    {
                        Console.Error.WriteLine($"No binary data found for attachment '{attachment.FileName}'.");
                        continue;
                    }

                    File.WriteAllBytes(outputPath, data);
                    Console.WriteLine($"Extracted image: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
