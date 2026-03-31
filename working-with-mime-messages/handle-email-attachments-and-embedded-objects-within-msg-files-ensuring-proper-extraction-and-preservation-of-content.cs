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
            string outputDir = "Extracted";

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

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Extract regular attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string fileName = string.IsNullOrEmpty(attachment.FileName) ? "attachment.bin" : attachment.FileName;
                    string safePath = Path.Combine(outputDir, fileName);
                    try
                    {
                        attachment.Save(safePath);
                        Console.WriteLine($"Saved attachment: {safePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {fileName}: {ex.Message}");
                    }
                }

                // Extract embedded images identified by MIME tag starting with "image/"
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (!string.IsNullOrEmpty(attachment.MimeTag) && attachment.MimeTag.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        string fileName = string.IsNullOrEmpty(attachment.FileName) ? "embedded_image.bin" : attachment.FileName;
                        string safePath = Path.Combine(outputDir, "Embedded_" + fileName);
                        try
                        {
                            attachment.Save(safePath);
                            Console.WriteLine($"Saved embedded image: {safePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save embedded image {fileName}: {ex.Message}");
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
