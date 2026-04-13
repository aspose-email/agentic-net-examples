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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // If the input MSG file does not exist, create a minimal placeholder
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder",
                    "This is a placeholder message."))
                {
                    placeholder.Save(inputPath);
                    Console.WriteLine($"Placeholder MSG created at: {inputPath}");
                }
            }

            // Load the MSG file, preserve its flags, and save it back unchanged
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Preserve original dates when saving
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Message saved to: {outputPath}");
                Console.WriteLine($"Message flags preserved: {message.Flags}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
