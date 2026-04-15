using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a MapiCalendar event
            MapiCalendar calendar = new MapiCalendar(
                "Conference Room",
                "Team Meeting",
                "Discuss project status",
                new DateTime(2023, 10, 10, 9, 0, 0),
                new DateTime(2023, 10, 10, 10, 0, 0));

            // Retrieve the Pacific Standard Time zone information
            TimeZoneInfo pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Create a MapiCalendarTimeZone from the TimeZoneInfo
            MapiCalendarTimeZone mapiTimeZone = new MapiCalendarTimeZone(pacificTimeZone);

            // Assign the time zone to the calendar event
            calendar.StartDateTimeZone = mapiTimeZone;
            calendar.EndDateTimeZone = mapiTimeZone;

            Console.WriteLine("Calendar event created with Pacific Standard Time zone.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
