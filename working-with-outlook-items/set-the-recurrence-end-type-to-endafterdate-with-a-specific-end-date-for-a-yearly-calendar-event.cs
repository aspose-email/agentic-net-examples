using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple MAPI message (optional, just for demonstration).
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Yearly Meeting",
                "Please attend the yearly meeting."))
            {
                // Initialize the appointment (calendar) part of the message.
                MapiCalendar appointment = new MapiCalendar
                {
                    StartDate = new DateTime(2024, 1, 1, 9, 0, 0),
                    EndDate   = new DateTime(2024, 1, 1, 10, 0, 0),
                    Location  = "Conference Room",
                    Subject   = "Yearly Meeting",
                    Body      = "Please attend the yearly meeting."
                };

                // Set up recurrence for a yearly event.
                var pattern = new MapiCalendarYearlyAndMonthlyRecurrencePattern
                {
                    // Define the recurrence as yearly.
                    PatternType = MapiCalendarRecurrencePatternType.Month, // Yearly handled via Period = 12
                    Period      = 12, // Repeat every 12 months (yearly)
                    StartDate   = appointment.StartDate,

                    // Set the end type to EndAfterDate with a specific end date.
                    EndType = MapiCalendarRecurrenceEndType.EndAfterDate,
                    EndDate = new DateTime(2028, 12, 31)
                };

                var recurrence = new MapiCalendarEventRecurrence
                {
                    RecurrencePattern = pattern
                };

                appointment.Recurrence = recurrence;

                // (Optional) Attach the appointment to the message if needed.
                // Note: MapiMessage does not expose an Appointment property in newer versions,
                // so we skip attaching it directly. The appointment can be saved separately.

                // Output confirmation.
                Console.WriteLine("Recurrence end type set to EndAfterDate with end date: " +
                                  pattern.EndDate.ToShortDateString());

                // (Optional) Save the appointment to a .msg file for verification.
                // appointment.Save("YearlyMeeting.msg");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
