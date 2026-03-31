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
            string msgPath = "sample.msg";
            string outputDir = "Attachments";

            // Verify input MSG file exists
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

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Retrieve attachment bytes via BinaryData
                    byte[] data = attachment.BinaryData;
                    if (data == null || data.Length == 0)
                        continue;

                    // Determine a safe file name
                    string fileName = !string.IsNullOrEmpty(attachment.LongFileName)
                        ? attachment.LongFileName
                        : attachment.FileName;

                    foreach (char invalid in Path.GetInvalidFileNameChars())
                        fileName = fileName.Replace(invalid, '_');

                    string outPath = Path.Combine(outputDir, fileName);

                    try
                    {
                        File.WriteAllBytes(outPath, data);
                        Console.WriteLine($"Saved attachment: {outPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{fileName}': {ex.Message}");
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
