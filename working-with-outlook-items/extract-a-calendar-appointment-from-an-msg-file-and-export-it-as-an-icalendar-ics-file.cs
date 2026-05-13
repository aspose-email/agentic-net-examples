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
            string inputMsgPath = "calendar.msg";
            string outputIcsPath = "appointment.ics";

            // Verify input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(inputMsgPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputMsgPath}' not found.");
                // Create minimal placeholder iCalendar file
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                    File.WriteAllText(outputIcsPath, placeholder);
                    Console.WriteLine($"Placeholder iCalendar created at '{outputIcsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar: {ex.Message}");
                }
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputIcsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Check if the MSG contains a calendar item
                if (mapiMessage.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                    // Create minimal placeholder iCalendar file
                    try
                    {
                        string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                        File.WriteAllText(outputIcsPath, placeholder);
                        Console.WriteLine($"Placeholder iCalendar created at '{outputIcsPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder iCalendar: {ex.Message}");
                    }
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar mapiCalendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();
                using (mapiCalendar)
                {
                    // Save as iCalendar (.ics)
                    try
                    {
                        mapiCalendar.Save(outputIcsPath);
                        Console.WriteLine($"Calendar exported successfully to '{outputIcsPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save iCalendar file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
