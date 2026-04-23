using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output file path
            string outputPath = "weekly_appointment.ics";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Appointment times
            DateTime start = new DateTime(2024, 5, 6, 9, 0, 0);
            DateTime end = start.AddHours(1);

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                "Weekly Sync",
                "Team weekly sync meeting",
                start,
                end,
                organizer,
                attendees);

            // Configure weekly recurrence
            WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(DateTime.Today, 1);
            recurrence.Interval = 1; // every week
            recurrence.EndDate = start.AddMonths(2); // end after two months

            appointment.Recurrence = recurrence;

            // Additional details
            appointment.Summary = "Weekly Sync";
            appointment.Description = "Team weekly sync meeting";

            // Save the appointment to an iCalendar file
            appointment.Save(outputPath, AppointmentSaveFormat.Ics);
            Console.WriteLine($"Appointment saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
