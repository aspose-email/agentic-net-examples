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
            // Paths for input MSG, output MSG, and folder containing replacement images
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string replacementImagesFolder = "ReplacementImages";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
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

                MapiMessage placeholderMsg = new MapiMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder message.");
                placeholderMsg.Save(inputMsgPath);
                Console.WriteLine($"Placeholder MSG created at {inputMsgPath}");
            }

            // Ensure the replacement images folder exists
            if (!Directory.Exists(replacementImagesFolder))
            {
                Directory.CreateDirectory(replacementImagesFolder);
                Console.WriteLine($"Created replacement images folder at {replacementImagesFolder}");
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Identify embedded images by MIME tag or common image file extensions
                    bool isImage = false;
                    if (!string.IsNullOrEmpty(attachment.MimeTag) &&
                        attachment.MimeTag.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        isImage = true;
                    }
                    else
                    {
                        string ext = Path.GetExtension(attachment.FileName);
                        if (!string.IsNullOrEmpty(ext))
                        {
                            string lowerExt = ext.ToLowerInvariant();
                            if (lowerExt == ".png" || lowerExt == ".jpg" || lowerExt == ".jpeg" ||
                                lowerExt == ".gif" || lowerExt == ".bmp")
                            {
                                isImage = true;
                            }
                        }
                    }

                    if (isImage)
                    {
                        // Look for a replacement image with the same file name
                        string replacementPath = Path.Combine(replacementImagesFolder, attachment.FileName);
                        if (File.Exists(replacementPath))
                        {
                            try
                            {
                                byte[] newData = File.ReadAllBytes(replacementPath);
                                // Replace the attachment content while preserving its original format
                                attachment.BinaryData = newData;
                                Console.WriteLine($"Replaced image attachment: {attachment.FileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to replace image {attachment.FileName}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No replacement found for {attachment.FileName}, keeping original.");
                        }
                    }
                }

                // Save the modified MSG file
                msg.Save(outputMsgPath);
                Console.WriteLine($"Modified MSG saved to {outputMsgPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
