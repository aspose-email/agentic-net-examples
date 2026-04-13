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
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Create the appointment (not disposable, so no using)
            Appointment appointment = new Appointment(
                location: "Conference Room",
                summary: "Team Sync",
                description: "Discuss project updates",
                startDate: new DateTime(2024, 5, 1, 10, 0, 0),
                endDate: new DateTime(2024, 5, 1, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees
            );

            // Define a daily recurrence pattern with a two‑day interval
            DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(5, 1);
            dailyPattern.Interval = 2; // repeats every 2 days
            // Optionally set an end date
            dailyPattern.EndDate = new DateTime(2024, 5, 31);

            // Assign the recurrence to the appointment
            appointment.Recurrence = dailyPattern;

            // Define output path
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            string icsPath = Path.Combine(outputDir, "RecurringMeeting.ics");

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(icsPath);
                Console.WriteLine($"Appointment saved to: {icsPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
