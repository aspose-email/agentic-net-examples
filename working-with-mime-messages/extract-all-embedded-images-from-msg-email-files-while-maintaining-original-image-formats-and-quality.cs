using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            string outputDir = "ExtractedImages";

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment att in msg.Attachments)
                {
                    string extension = Path.GetExtension(att.FileName)?.ToLowerInvariant();

                    bool isImage = extension == ".png" ||
                                   extension == ".jpg" ||
                                   extension == ".jpeg" ||
                                   extension == ".gif" ||
                                   extension == ".bmp" ||
                                   extension == ".tiff";

                    if (!isImage)
                        continue;

                    string outPath = Path.Combine(outputDir, att.FileName);
                    int duplicateIndex = 1;

                    while (File.Exists(outPath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(att.FileName);
                        outPath = Path.Combine(outputDir, $"{nameWithoutExt}_{duplicateIndex}{extension}");
                        duplicateIndex++;
                    }

                    att.Save(outPath);
                    Console.WriteLine($"Saved image: {outPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
