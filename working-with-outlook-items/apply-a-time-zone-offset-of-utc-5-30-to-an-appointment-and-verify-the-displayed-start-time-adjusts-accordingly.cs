using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Define start and end times in UTC
            DateTime startUtc = new DateTime(2023, 10, 1, 9, 0, 0, DateTimeKind.Utc);
            DateTime endUtc = startUtc.AddHours(1);

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                startUtc,
                endUtc,
                new MailAddress("organizer@example.com"),
                attendees);

            // Show original start time (UTC)
            Console.WriteLine("Original Start (UTC): " + appointment.StartDate.ToString("u"));

            // Apply UTC+5:30 time zone (India Standard Time / Asia/Kolkata)
            appointment.SetTimeZone("Asia/Kolkata");

            // Resolve the time zone identifier
            TimeZoneInfo targetTimeZone = null;
            try
            {
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback for Windows identifiers
                targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }

            // Convert the start time to the target time zone
            DateTime startInTargetZone = TimeZoneInfo.ConvertTimeFromUtc(startUtc, targetTimeZone);
            Console.WriteLine("Start Time in UTC+5:30: " + startInTargetZone.ToString("u"));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
