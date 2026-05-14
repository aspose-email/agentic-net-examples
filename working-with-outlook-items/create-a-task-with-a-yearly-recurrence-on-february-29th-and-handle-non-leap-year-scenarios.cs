using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Determine a start date that is valid for the current year.
            int year = DateTime.Now.Year;
            DateTime startDate;
            if (DateTime.IsLeapYear(year))
                startDate = new DateTime(year, 2, 29, 10, 0, 0);
            else
                startDate = new DateTime(year, 2, 28, 10, 0, 0); // fallback for non‑leap years

            // Create a yearly recurrence pattern that targets February 29.
            YearlyRecurrencePattern yearlyPattern = new YearlyRecurrencePattern(29, CalendarMonth.February);

            // Build the appointment and assign the recurrence.
            Appointment appointment = new Appointment(
                "Team Sync",
                startDate,
                startDate.AddHours(1),
                new MailAddress("organizer@example.com"),
                new MailAddressCollection());

            appointment.Recurrence = yearlyPattern;
            appointment.Summary = "Quarterly team sync (occurs on Feb 29)";
            appointment.Description = "This meeting recurs yearly on February 29. In non‑leap years it will be scheduled on February 28.";

            // Define output path.
            string outputPath = Path.Combine(Environment.CurrentDirectory, "YearlyFeb29Appointment.ics");

            // Ensure the directory exists.
            string directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Save the appointment to an iCalendar file.
            try
            {
                appointment.Save(outputPath, AppointmentSaveFormat.Ics);
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
