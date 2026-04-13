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
            // Define the output file path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appointment.ics");

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Organizer
            MailAddress organizer = new MailAddress("organizer@example.com");

            // Define start and end times for the first occurrence
            DateTime startDate = new DateTime(2024, 5, 14, 10, 0, 0); // Example second Tuesday of May 2024
            DateTime endDate = startDate.AddHours(1);

            // Create a monthly recurrence pattern for the second Tuesday of each month
            MonthlyRecurrencePattern recurrence = new MonthlyRecurrencePattern(
                DayPosition.Second,   // Second occurrence
                CalendarDay.Tuesday, // Tuesday
                1);                   // Every month

            // Create the appointment with a custom agenda (summary and description)
            Appointment appointment = new Appointment(
                "Conference Room",          // Location
                "Team Sync Meeting",        // Summary (agenda)
                "Discuss project updates and next steps.", // Description
                startDate,
                endDate,
                organizer,
                attendees,
                recurrence);

            // Save the appointment to an iCalendar file
            appointment.Save(filePath, Aspose.Email.Calendar.AppointmentSaveFormat.Ics);

            Console.WriteLine($"Appointment saved to: {filePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
