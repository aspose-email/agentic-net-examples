using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the RRULE string for a monthly recurrence (e.g., every month, 5 occurrences)
            string rrule = "FREQ=MONTHLY;INTERVAL=1;COUNT=5";

            // Parse the RRULE manually to create a MonthlyRecurrencePattern
            MonthlyRecurrencePattern recurrence = ParseMonthlyRRule(rrule);

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Team Sync",
                new DateTime(2024, 5, 1, 10, 0, 0),
                new DateTime(2024, 5, 1, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            // Apply the recurrence pattern
            appointment.Recurrence = recurrence;

            // Define output path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "RecurringAppointment.ics");

            // Ensure the directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment to an iCalendar file
            appointment.Save(outputPath, Aspose.Email.Calendar.AppointmentSaveFormat.Ics);
            Console.WriteLine($"Appointment saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Simple RRULE parser for monthly recurrence patterns
    private static MonthlyRecurrencePattern ParseMonthlyRRule(string rrule)
    {
        MonthlyRecurrencePattern pattern = new MonthlyRecurrencePattern();

        // Split the rule into components
        string[] parts = rrule.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            string[] kv = part.Split('=', 2);
            if (kv.Length != 2) continue;

            string key = kv[0].Trim().ToUpperInvariant();
            string value = kv[1].Trim();

            switch (key)
            {
                case "INTERVAL":
                    if (int.TryParse(value, out int interval))
                        pattern.Interval = interval;
                    break;
                case "COUNT":
                    if (int.TryParse(value, out int count))
                        pattern.Occurs = count;
                    break;
                case "UNTIL":
                    if (DateTime.TryParse(value, out DateTime until))
                        pattern.EndDate = until;
                    break;
                // Additional RRULE components can be handled here if needed
            }
        }

        // Set a start offset (default 0) if not already set
        if (pattern.StartOffset == 0)
            pattern.StartOffset = 0;

        return pattern;
    }
}
