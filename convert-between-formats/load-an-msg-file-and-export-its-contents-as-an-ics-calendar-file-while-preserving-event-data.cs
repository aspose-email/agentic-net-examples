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
            string inputPath = "calendar.msg";
            string outputPath = "calendar.ics";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                    {
                        calendar.Save(outputPath);
                        Console.WriteLine($"ICS file saved to {outputPath}");
                    }
                }
                else
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
