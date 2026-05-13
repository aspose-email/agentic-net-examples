using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file containing a calendar item
            string inputMsgPath = "calendar.msg";
            // Output ICS file path
            string outputIcsPath = "exported.ics";

            // Guard input file existence
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

                try
                {
                    var placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));

                    placeholderCalendar.Save(inputMsgPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputMsgPath}' does not exist.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputIcsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Variables to hold original UTC timestamps for later comparison
            DateTime originalStartUtc = DateTime.MinValue;
            DateTime originalEndUtc = DateTime.MinValue;

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Verify that the message is a calendar item
                if (mapiMessage.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(outputIcsPath, ".ics");
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
                MapiCalendar mapiCalendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                // Preserve original timestamps for comparison (assume they are local)
                originalStartUtc = DateTime.SpecifyKind(mapiCalendar.StartDate, DateTimeKind.Local).ToUniversalTime();
                originalEndUtc = DateTime.SpecifyKind(mapiCalendar.EndDate, DateTimeKind.Local).ToUniversalTime();

                // Convert calendar times to UTC for export
                mapiCalendar.StartDate = DateTime.SpecifyKind(originalStartUtc, DateTimeKind.Utc);
                mapiCalendar.EndDate = DateTime.SpecifyKind(originalEndUtc, DateTimeKind.Utc);

                // Save as iCalendar (ICS) using default options
                MapiCalendarSaveOptions saveOptions = MapiCalendarSaveOptions.DefaultIcs;
                mapiCalendar.Save(outputIcsPath, saveOptions);
            }

            // Load the generated ICS file as an Appointment
            Appointment loadedAppointment = Appointment.Load(outputIcsPath);

            // Compare timestamps (both are in UTC)
            DateTime exportedStartUtc = loadedAppointment.StartDate.ToUniversalTime();
            DateTime exportedEndUtc = loadedAppointment.EndDate.ToUniversalTime();

            Console.WriteLine($"Original Start (UTC): {originalStartUtc:O}");
            Console.WriteLine($"Exported Start (UTC): {exportedStartUtc:O}");
            Console.WriteLine($"Original End   (UTC): {originalEndUtc:O}");
            Console.WriteLine($"Exported End   (UTC): {exportedEndUtc:O}");

            bool startMatches = originalStartUtc == exportedStartUtc;
            bool endMatches = originalEndUtc == exportedEndUtc;

            Console.WriteLine($"Start timestamps match: {startMatches}");
            Console.WriteLine($"End timestamps match: {endMatches}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
