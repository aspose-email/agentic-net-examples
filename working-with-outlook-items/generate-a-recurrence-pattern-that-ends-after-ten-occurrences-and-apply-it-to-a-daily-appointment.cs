using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define appointment details
            DateTime startDate = new DateTime(2023, 10, 1, 9, 0, 0);
            DateTime endDate = startDate.AddHours(1);
            MailAddress organizer = new MailAddress("organizer@example.com");

            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create a daily recurrence that ends after ten occurrences
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(10);
            // Interval defaults to 1 (every day)

            // Create the appointment with the recurrence pattern
            Appointment appointment = new Appointment(
                "Conference Room",
                "Daily Standup",
                "Team sync meeting",
                startDate,
                endDate,
                organizer,
                attendees,
                recurrence);

            // Example output
            Console.WriteLine("Appointment created with a daily recurrence of {0} occurrences.", recurrence.Occurs);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
