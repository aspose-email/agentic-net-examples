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
            // Define appointment details
            string location = "Conference Room A";
            string summary = "Project Sync Meeting";
            string description = "Agenda:\n1. Review progress\n2. Discuss blockers\n3. Plan next steps";

            DateTime startDate = new DateTime(2026, 5, 1, 10, 0, 0);
            DateTime endDate = startDate.AddHours(1);

            MailAddress organizer = new MailAddress("organizer@example.com");

            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Daily recurrence pattern (every day)
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(startDate, 1);

            // Create the appointment with recurrence
            Appointment appointment = new Appointment(
                location,
                summary,
                description,
                startDate,
                endDate,
                organizer,
                attendees,
                recurrence);

            // Prepare output path
            string outputPath = "RecurringAppointment.ics";
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(outputPath);
                Console.WriteLine($"Appointment saved to '{outputPath}'.");
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
