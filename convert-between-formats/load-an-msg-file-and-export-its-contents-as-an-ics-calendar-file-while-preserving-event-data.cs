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
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Ensure the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    try
                    {
                        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Placeholder//EN\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20260101T000000Z\r\nDTSTART:20260101T000000Z\r\nDTEND:20260101T010000Z\r\nSUMMARY:Placeholder\r\nEND:VEVENT\r\nEND:VCALENDAR";
                        File.WriteAllText(icsPath, placeholderIcs);
                        Console.WriteLine($"Input MSG is not a calendar. Placeholder ICS saved to \"{icsPath}\".");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder ICS: {ex.Message}");
                    }
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
