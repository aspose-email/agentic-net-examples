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
            string replacementImagePath = "newImage.png";
            string outputMsgPath = "output.msg";

            // Guard input MSG file existence
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

                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }

            // Guard replacement image existence
            if (!File.Exists(replacementImagePath))
            {
                Console.Error.WriteLine($"Replacement image file not found: {replacementImagePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Iterate over attachments to find embedded images
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Identify image attachments by file extension (common image types)
                    string fileName = attachment.FileName ?? string.Empty;
                    string extension = Path.GetExtension(fileName).ToLowerInvariant();

                    bool isImage = extension == ".png" ||
                                   extension == ".jpg" ||
                                   extension == ".jpeg" ||
                                   extension == ".gif" ||
                                   extension == ".bmp";

                    // Additional check using MimeTag if available
                    if (!isImage && !string.IsNullOrEmpty(attachment.MimeTag))
                    {
                        isImage = attachment.MimeTag.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                    }

                    if (isImage)
                    {
                        try
                        {
                            // Replace image content with the new image bytes
                            byte[] newImageData = File.ReadAllBytes(replacementImagePath);
                            attachment.BinaryData = newImageData;
                            Console.WriteLine($"Replaced image attachment: {fileName}");
                        }
                        catch (Exception imgEx)
                        {
                            Console.Error.WriteLine($"Failed to replace image '{fileName}': {imgEx.Message}");
                        }
                    }
                }

                // Save the modified MSG
                try
                {
                    msg.Save(outputMsgPath);
                    Console.WriteLine($"Modified MSG saved to: {outputMsgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save modified MSG: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
