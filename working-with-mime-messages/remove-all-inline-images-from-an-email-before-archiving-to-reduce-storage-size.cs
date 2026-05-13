using System;
using System.IO;
using System.Collections.Generic;
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

            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Identify inline image attachments (common image extensions)
                List<MapiAttachment> attachmentsToRemove = new List<MapiAttachment>();
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string extension = Path.GetExtension(attachment.FileName);
                    if (extension != null)
                    {
                        string extLower = extension.ToLowerInvariant();
                        bool isImage = extLower == ".png" || extLower == ".jpg" || extLower == ".jpeg" || extLower == ".gif" || extLower == ".bmp";
                        if (isImage)
                        {
                            attachmentsToRemove.Add(attachment);
                        }
                    }
                }

                // Remove identified inline images
                foreach (MapiAttachment att in attachmentsToRemove)
                {
                    message.Attachments.Remove(att);
                }

                // Save the cleaned message
                message.Save(outputPath);
                Console.WriteLine($"Inline images removed and saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
