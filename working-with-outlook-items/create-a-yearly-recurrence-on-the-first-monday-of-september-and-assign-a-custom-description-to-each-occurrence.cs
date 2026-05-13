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
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Define the start and end time for the first occurrence
            DateTime start = new DateTime(DateTime.Now.Year, 9, 1, 9, 0, 0);
            DateTime end = start.AddHours(1);

            // Yearly recurrence on the first Monday of September
            YearlyRecurrencePattern yearlyPattern = new YearlyRecurrencePattern(
                CalendarDay.Monday,
                CalendarMonth.September,
                DayPosition.First);

            // Create the appointment with a custom description
            Appointment appointment = new Appointment(
                "Conference Room",
                "Annual Meeting",
                "Custom description for each occurrence",
                start,
                end,
                new MailAddress("organizer@example.com"),
                attendees,
                yearlyPattern);

            // Ensure the output directory exists
            string filePath = "annual_meeting.ics";
            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment as an iCalendar file
            appointment.Save(filePath);
            Console.WriteLine($"Appointment saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
