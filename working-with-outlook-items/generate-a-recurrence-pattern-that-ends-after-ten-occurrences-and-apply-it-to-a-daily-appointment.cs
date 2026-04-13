using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

namespace RecurrenceExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create a daily appointment
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Team Meeting",
                    "Discuss project updates",
                    new DateTime(2023, 10, 1, 9, 0, 0),
                    new DateTime(2023, 10, 1, 10, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Define a daily recurrence that ends after ten occurrences
                DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(1); // every 1 day
                dailyPattern.Occurs = 10; // limit to 10 occurrences

                // Apply the recurrence to the appointment
                appointment.Recurrence = dailyPattern;

                Console.WriteLine("Appointment created with daily recurrence for 10 occurrences.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
