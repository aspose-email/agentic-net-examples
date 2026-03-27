using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
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

                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string extension = Path.GetExtension(attachment.FileName)?.ToLowerInvariant();

                        if (extension == ".png" ||
                            extension == ".jpg" ||
                            extension == ".jpeg" ||
                            extension == ".gif" ||
                            extension == ".bmp")
                        {
                            string outputPath = attachment.FileName;

                            try
                            {
                                attachment.Save(outputPath);
                                Console.WriteLine($"Saved image: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
}
