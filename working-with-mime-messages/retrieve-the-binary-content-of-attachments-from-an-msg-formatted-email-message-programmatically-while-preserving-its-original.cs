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
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            string outputDir = "Attachments";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string fileName = attachment.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "UnnamedAttachment.bin";
                    }
                    string outputPath = Path.Combine(outputDir, fileName);
                    try
                    {
                        File.WriteAllBytes(outputPath, attachment.BinaryData);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {fileName}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
