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
            // Define file paths
            string inputMsgPath = "input.msg";
            string replacementImagePath = "replacement.jpg";
            string outputMsgPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure the replacement image exists; create an empty placeholder if missing
            if (!File.Exists(replacementImagePath))
            {
                try
                {
                    // Write a minimal JPEG header (just to have a valid file)
                    byte[] jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46,
                                                     0x49, 0x46, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01,
                                                     0x00, 0x01, 0x00, 0x00 };
                    File.WriteAllBytes(replacementImagePath, jpegHeader);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Iterate over attachments and replace embedded images
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    bool isImage = false;

                    // Check MIME tag for image type
                    if (!string.IsNullOrEmpty(attachment.MimeTag) &&
                        attachment.MimeTag.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                    {
                        isImage = true;
                    }
                    else if (!string.IsNullOrEmpty(attachment.FileName))
                    {
                        // Fallback: check file extension
                        string ext = Path.GetExtension(attachment.FileName);
                        if (ext != null)
                        {
                            string lowerExt = ext.ToLowerInvariant();
                            if (lowerExt == ".png" || lowerExt == ".jpg" ||
                                lowerExt == ".jpeg" || lowerExt == ".gif" ||
                                lowerExt == ".bmp")
                            {
                                isImage = true;
                            }
                        }
                    }

                    if (isImage)
                    {
                        try
                        {
                            // Replace the binary data with the CDN image content
                            attachment.BinaryData = File.ReadAllBytes(replacementImagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to replace image for attachment '{attachment.FileName}': {ex.Message}");
                            // Continue processing other attachments
                        }
                    }
                }

                // Save the modified MSG
                try
                {
                    msg.Save(outputMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified MSG: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
