using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;

namespace RetrieveEmbeddedImages
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source MSG file
                string msgPath = "input.msg";

                // Verify that the MSG file exists before attempting to load it
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

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                // Load the Outlook MSG file
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Directory where extracted images will be saved
                string outputDirectory = "ExtractedImages";

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Iterate through all attachments and save those that are image files
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Skip attachments without a file name
                    if (string.IsNullOrEmpty(attachment.FileName))
                        continue;

                    // Determine the file extension in lower case
                    string extension = Path.GetExtension(attachment.FileName).ToLowerInvariant();

                    // List of supported image extensions
                    bool isImage = extension == ".jpg" ||
                                   extension == ".jpeg" ||
                                   extension == ".png" ||
                                   extension == ".gif" ||
                                   extension == ".bmp" ||
                                   extension == ".tiff" ||
                                   extension == ".ico";

                    if (!isImage)
                        continue;

                    // Build the full path for the extracted image
                    string outputPath = Path.Combine(outputDirectory, attachment.FileName);

                    // Save the attachment preserving its original format using BinaryData
                    if (attachment.BinaryData != null && attachment.BinaryData.Length > 0)
                    {
                        File.WriteAllBytes(outputPath, attachment.BinaryData);
                        Console.WriteLine($"Saved image: {outputPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Attachment '{attachment.FileName}' has no binary data.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
