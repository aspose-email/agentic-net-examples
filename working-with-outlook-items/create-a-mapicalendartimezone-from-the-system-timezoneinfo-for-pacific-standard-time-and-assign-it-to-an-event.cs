using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Retrieve the Pacific Standard Time zone from the system.
            TimeZoneInfo pacificTimeZone;
            try
            {
                pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException ex)
            {
                Console.Error.WriteLine("Pacific Standard Time zone not found: " + ex.Message);
                return;
            }
            catch (InvalidTimeZoneException ex)
            {
                Console.Error.WriteLine("Invalid time zone data: " + ex.Message);
                return;
            }

            // Create a MapiCalendarTimeZone based on the retrieved TimeZoneInfo.
            MapiCalendarTimeZone calendarTimeZone = new MapiCalendarTimeZone(pacificTimeZone);
            calendarTimeZone.KeyName = "Pacific Standard Time";

            // Create a calendar event and assign the time zone to it.
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.Subject = "Sample Event";
                calendar.StartDate = new DateTime(2023, 10, 1, 9, 0, 0);
                calendar.EndDate = new DateTime(2023, 10, 1, 10, 0, 0);
                calendar.StartDateTimeZone = calendarTimeZone;
                calendar.EndDateTimeZone = calendarTimeZone;

                Console.WriteLine("Event created with time zone: " + calendar.StartDateTimeZone.KeyName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
