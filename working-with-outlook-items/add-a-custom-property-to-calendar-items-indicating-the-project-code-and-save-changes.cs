using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "calendar.msg";
            string outputPath = "calendar_updated.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder Calendar", "Body"))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the message and verify it is a calendar item
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(outputPath, ".ics");
                    try
                    {
                        File.WriteAllText(placeholderIcsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                        Console.WriteLine($"Input MSG is not a calendar item. Placeholder ICS created at {placeholderIcsPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing placeholder ICS: {ex.Message}");
                    }
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Add custom property for project code
                const string projectCode = "PRJ123";
                byte[] projectCodeBytes = Encoding.Unicode.GetBytes(projectCode);
                const long customTag = 0x8000; // Example custom property tag
                calendar.SetProperty(new MapiProperty(customTag, projectCodeBytes));

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the updated calendar
                try
                {
                    calendar.Save(outputPath);
                    Console.WriteLine($"Calendar saved with custom property to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save updated calendar: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
