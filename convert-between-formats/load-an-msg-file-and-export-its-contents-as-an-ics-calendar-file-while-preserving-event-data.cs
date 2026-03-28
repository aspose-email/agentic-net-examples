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
            string msgPath = "input.msg";
            string icsPath = "output.ics";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file \"{msgPath}\" not found.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Ensure the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The provided MSG file does not contain a calendar.");
                    return;
                }

                // Convert to MapiCalendar
                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    // Prepare save options for iCalendar (ICS) format
                    MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions();

                    // Save the calendar as an .ics file
                    calendar.Save(icsPath, saveOptions);
                    Console.WriteLine($"Calendar exported successfully to \"{icsPath}\".");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
