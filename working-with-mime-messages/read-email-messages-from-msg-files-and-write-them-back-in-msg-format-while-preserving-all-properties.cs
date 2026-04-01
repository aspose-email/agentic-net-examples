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

            // Verify input file exists
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

                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Could not create output directory – {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage mapMessage;
            try
            {
                mapMessage = MapiMessage.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error: Failed to load MSG file – {loadEx.Message}");
                return;
            }

            using (mapMessage)
            {
                // Set save options to preserve original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    mapMessage.Save(outputPath, saveOptions);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error: Failed to save MSG file – {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
