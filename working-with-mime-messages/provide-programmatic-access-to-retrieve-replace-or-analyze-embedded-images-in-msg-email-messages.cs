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
            // Paths to the original MSG file and the replacement image.
            string msgPath = "sample.msg";
            string newImagePath = "newImage.png";

            // Verify that the required files exist.
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            if (!File.Exists(newImagePath))
            {
                Console.Error.WriteLine($"Replacement image not found: {newImagePath}");
                return;
            }

            // Load the MSG message.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");

                // Iterate through all attachments (including embedded images).
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}, Inline: {attachment.IsInline}");

                    // Analyze the attachment size if binary data is present.
                    if (attachment.BinaryData != null)
                    {
                        Console.WriteLine($"Size: {attachment.BinaryData.Length} bytes");
                    }

                    // Replace the attachment if it is an image (png, jpg, jpeg).
                    string extension = Path.GetExtension(attachment.FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        string extLower = extension.ToLowerInvariant();
                        if (extLower == ".png" || extLower == ".jpg" || extLower == ".jpeg")
                        {
                            byte[] newData = File.ReadAllBytes(newImagePath);
                            attachment.BinaryData = newData;
                            Console.WriteLine($"Replaced image attachment '{attachment.FileName}' with new image.");
                        }
                    }
                }

                // Save the modified message to a new file.
                string outPath = "modified.msg";
                msg.Save(outPath);
                Console.WriteLine($"Modified message saved to {outPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
