using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Define start and end times (local time)
            DateTime start = new DateTime(2023, 1, 1, 9, 0, 0);
            DateTime end = new DateTime(2023, 1, 1, 10, 0, 0);

            // Create the appointment
            Appointment appointment = new Appointment("Team Meeting", start, end, organizer, attendees);

            // Apply UTC+5:30 time zone to the appointment
            string utcPlus530 = "UTC+05:30";
            appointment.StartTimeZone = utcPlus530;
            appointment.EndTimeZone = utcPlus530;

            // Display the original start time (as set)
            Console.WriteLine($"Original Start (local): {appointment.StartDate}");

            // Compute and display the start time in UTC based on the offset
            TimeSpan offset = new TimeSpan(5, 30, 0);
            DateTime utcStart = appointment.StartDate - offset;
            Console.WriteLine($"Start Time in UTC (adjusted): {utcStart} (Offset {offset})");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
