using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define attendees
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("person1@domain.com"),
                new MailAddress("person2@domain.com")
            };

            // Create a daily recurrence pattern with a 2‑day interval
            DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(2); // repeats every 2 days

            // Create the appointment (meeting request) without recurrence
            Appointment meeting = new Appointment(
                location: "Conference Room",
                startDate: new DateTime(2024, 6, 1, 10, 0, 0),
                endDate: new DateTime(2024, 6, 1, 11, 0, 0),
                organizer: new MailAddress("organizer@domain.com"),
                attendees: attendees);

            // Assign recurrence pattern
            meeting.Recurrence = dailyPattern;

            meeting.Summary = "Team Sync";
            meeting.Description = "Recurring team sync meeting every two days.";

            // Prepare output path
            string outputPath = "meeting.ics";
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment to an iCalendar file
            meeting.Save(outputPath, AppointmentSaveFormat.Ics);
            Console.WriteLine($"Meeting request saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
