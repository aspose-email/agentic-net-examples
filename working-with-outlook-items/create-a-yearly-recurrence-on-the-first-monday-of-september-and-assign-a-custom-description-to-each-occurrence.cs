using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define attendees for the appointment
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create a yearly recurrence pattern for the first Monday of September
            YearlyRecurrencePattern yearlyPattern = new YearlyRecurrencePattern(
                CalendarDay.Monday,
                CalendarMonth.September,
                DayPosition.First);

            // Limit the recurrence to a specific end date (e.g., 5 years from now)
            yearlyPattern.EndDate = new DateTime(DateTime.Now.Year + 5, 9, 30);

            // Create the appointment with the recurrence pattern
            Appointment appointment = new Appointment(
                "Conference Room",
                "Annual Meeting",
                "Custom description for each occurrence",
                new DateTime(DateTime.Now.Year, 9, 1, 9, 0, 0),
                new DateTime(DateTime.Now.Year, 9, 1, 10, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees,
                yearlyPattern);

            // Output appointment details
            Console.WriteLine("Appointment created with yearly recurrence on the first Monday of September.");
            Console.WriteLine($"Summary: {appointment.Summary}");
            Console.WriteLine($"Description: {appointment.Description}");
            Console.WriteLine($"Recurrence ends on: {yearlyPattern.EndDate:d}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
