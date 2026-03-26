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
            // Define the output file path for the appointment
            string filePath = "appointment.ics";
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Define the organizer
            MailAddress organizer = new MailAddress("organizer@example.com");

            // Define a daily recurrence pattern (every 1 day)
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(1);
            // Uncomment the following line if the property is available in the target version
            // recurrence.Occurrences = 5;

            // Create the appointment with recurrence
            Appointment appointment = new Appointment(
                "Conference Room",
                "Team Meeting",
                "Weekly sync meeting",
                DateTime.Now.AddDays(1).AddHours(9),
                DateTime.Now.AddDays(1).AddHours(10),
                organizer,
                attendees,
                recurrence);

            // Save the appointment to an iCalendar file
            appointment.Save(filePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}