using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailAttachmentExtractor
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

                    Console.Error.WriteLine($"Input file not found: {msgPath}");
                    return;
                }

                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    if (msg.Attachments == null || msg.Attachments.Count == 0)
                    {
                        Console.WriteLine("No attachments found in the MSG file.");
                        return;
                    }

                    string outputDir = "Attachments";

                    try
                    {
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {dirEx.Message}");
                        return;
                    }

                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string originalFileName = attachment.FileName;
                        if (string.IsNullOrEmpty(originalFileName))
                        {
                            originalFileName = "attachment.bin";
                        }

                        // Sanitize file name
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        string safeFileName = originalFileName;
                        foreach (char c in invalidChars)
                        {
                            safeFileName = safeFileName.Replace(c, '_');
                        }

                        string outputPath = Path.Combine(outputDir, safeFileName);

                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved attachment to: {outputPath}");
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{originalFileName}': {attEx.Message}");
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
}
