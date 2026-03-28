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
            string msgPath = "sample.msg";
            string icsPath = "output.ics";

            // Ensure the input MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file '{msgPath}' does not exist.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Verify that the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Save the calendar as an iCalendar (ICS) file
                calendar.Save(icsPath, MapiCalendarSaveOptions.DefaultIcs);
                Console.WriteLine($"Calendar successfully saved to '{icsPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
