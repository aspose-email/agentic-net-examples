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
            string inputPath = "calendar.msg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Verify that the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Display some calendar information
                Console.WriteLine($"Subject: {calendar.Subject}");
                Console.WriteLine($"Location: {calendar.Location}");
                Console.WriteLine($"Start: {calendar.StartDate}");
                Console.WriteLine($"End: {calendar.EndDate}");

                // Save the calendar as an iCalendar file
                string outputPath = "output.ics";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                try
                {
                    calendar.Save(outputPath);
                    Console.WriteLine($"Calendar saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving calendar: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
