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
            string outputPath = "output.ics";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    placeholderCalendar.Save(inputPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            try
            {
                // Load the MSG file
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    // Check if the MSG contains a calendar item
                    if (msg.SupportedType == MapiItemType.Calendar)
                    {
                        // Convert to MapiCalendar
                        using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                        {
                            // Ensure required properties are set
                            if (string.IsNullOrEmpty(calendar.Subject))
                                calendar.Subject = "No Subject";

                            if (string.IsNullOrEmpty(calendar.Body))
                                calendar.Body = "No Body";

                            // Save as iCalendar (ICS) file
                            calendar.Save(outputPath);
                            Console.WriteLine($"Calendar saved to '{outputPath}'.");
                        }
                    }
                    else
                    {
                        // Not a calendar item – create a minimal placeholder ICS
                        string placeholder = "BEGIN:VCALENDAR\r\nEND:VCALENDAR";
                        File.WriteAllText(outputPath, placeholder);
                        Console.WriteLine($"Input MSG is not a calendar. Placeholder ICS created at '{outputPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
