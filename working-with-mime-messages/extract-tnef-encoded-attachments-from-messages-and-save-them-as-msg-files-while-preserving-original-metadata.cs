using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace TnefAttachmentExtractor
{
    // Author: Aspose.Email example for extracting TNEF attachments and saving as MSG files.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input TNEF file (e.g., winmail.dat) and output directory.
                string tnefFilePath = "winmail.dat";
                string outputDirectory = "ExtractedAttachments";

                // Guard file existence.
                if (!File.Exists(tnefFilePath))
                {
                    Console.Error.WriteLine($"Input TNEF file not found: {tnefFilePath}");
                    return;
                }

                // Ensure output directory exists.
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the TNEF message.
                using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tnefFilePath))
                {
                    // Iterate through each attachment.
                    foreach (MapiAttachment attachment in tnefMessage.Attachments)
                    {
                        // Build a safe file name.
                        string safeFileName = Path.GetFileName(attachment.FileName);
                        if (string.IsNullOrEmpty(safeFileName))
                        {
                            safeFileName = "attachment.dat";
                        }

                        string outputPath = Path.Combine(outputDirectory, safeFileName);

                        // Save the attachment. For embedded MSG files this will preserve original metadata.
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
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
