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
            string msgPath = "calendar.msg";
            string icsPath = "calendar.ics";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                if (mapiMessage.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("Error: The MSG file does not contain a calendar item.");
                    return;
                }

                MapiCalendar mapiCalendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                using (MapiCalendar calendarToSave = mapiCalendar)
                {
                    MapiCalendarIcsSaveOptions icsOptions = new MapiCalendarIcsSaveOptions();
                    calendarToSave.Save(icsPath, icsOptions);
                }
            }

            Console.WriteLine($"Successfully converted '{msgPath}' to '{icsPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
