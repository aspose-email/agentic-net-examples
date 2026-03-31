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
            string outputDir = "ExtractedImages";

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

                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating directory {outputDir}: {ex.Message}");
                    return;
                }
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                int imageIndex = 0;
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string mimeTag = attachment.MimeTag ?? string.Empty;
                    string fileName = attachment.FileName ?? $"image_{imageIndex}";
                    bool isImage = mimeTag.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                                   fileName.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase);

                    if (isImage)
                    {
                        // Access binary data via BinaryData property
                        byte[] imageData = attachment.BinaryData;
                        if (imageData != null && imageData.Length > 0)
                        {
                            string extension = Path.GetExtension(fileName);
                            if (string.IsNullOrEmpty(extension))
                            {
                                // Fallback to mime type if extension missing
                                string mimeSubtype = mimeTag.Substring("image/".Length);
                                extension = "." + mimeSubtype;
                            }

                            string outputPath = Path.Combine(outputDir, $"image_{imageIndex}{extension}");
                            try
                            {
                                File.WriteAllBytes(outputPath, imageData);
                                Console.WriteLine($"Extracted: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error writing file {outputPath}: {ex.Message}");
                            }

                            imageIndex++;
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
