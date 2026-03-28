using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace ExtractEmbeddedImages
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "sample.msg";
                string outputDirectory = "ExtractedImages";

                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                    return;
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string attachmentFileName = attachment.FileName;
                        if (string.IsNullOrEmpty(attachmentFileName))
                        {
                            continue;
                        }

                        string fileExtension = Path.GetExtension(attachmentFileName).ToLowerInvariant();
                        bool isImage = fileExtension == ".png" ||
                                       fileExtension == ".jpg" ||
                                       fileExtension == ".jpeg" ||
                                       fileExtension == ".gif" ||
                                       fileExtension == ".bmp" ||
                                       fileExtension == ".tiff";

                        if (!isImage)
                        {
                            continue;
                        }

                        string outputPath = Path.Combine(outputDirectory, attachmentFileName);
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved image: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
