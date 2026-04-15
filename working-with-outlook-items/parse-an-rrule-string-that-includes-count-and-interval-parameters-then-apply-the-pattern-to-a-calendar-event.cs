using Aspose.Email.Clients;
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
            // Define the RRULE string containing COUNT and INTERVAL
            string rrule = "FREQ=DAILY;COUNT=5;INTERVAL=2";

            // Parse the RRULE manually
            int count = 0;
            int interval = 1; // default interval
            string[] parts = rrule.Split(';');
            foreach (string part in parts)
            {
                string[] kv = part.Split('=');
                if (kv.Length != 2) continue;
                string key = kv[0].Trim().ToUpperInvariant();
                string value = kv[1].Trim();
                if (key == "COUNT")
                {
                    int.TryParse(value, out count);
                }
                else if (key == "INTERVAL")
                {
                    int.TryParse(value, out interval);
                }
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                "Team Sync",
                "Weekly team sync meeting",
                new DateTime(2023, 10, 2, 10, 0, 0),
                new DateTime(2023, 10, 2, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            // Build a daily recurrence pattern using the parsed COUNT and INTERVAL
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(5, 1);
            recurrence.Interval = interval;
            recurrence.Occurs = count; // number of occurrences
            appointment.Recurrence = recurrence;

            // Prepare output path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "appointment.ics");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(outputPath);
                Console.WriteLine($"Appointment saved to: {outputPath}");
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
        }
    }
}
