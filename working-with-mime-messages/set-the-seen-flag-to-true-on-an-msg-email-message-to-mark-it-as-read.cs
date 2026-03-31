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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com"))
                {
                    placeholder.Save(inputPath);
                    Console.Error.WriteLine($"Input file not found. Created placeholder MSG at '{inputPath}'.");
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG, set the Seen (Read) flag, and save
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Add the Read flag (MSGFLAG_READ) to mark as seen
                message.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);
                message.Save(outputPath);
                Console.WriteLine($"Message marked as read and saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
