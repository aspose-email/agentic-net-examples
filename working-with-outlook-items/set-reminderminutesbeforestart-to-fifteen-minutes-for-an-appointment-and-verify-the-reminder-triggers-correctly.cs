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
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee@example.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                "Team Meeting",
                "Discuss project status",
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                new MailAddress("organizer@example.com"),
                attendees);

            // Add the default 15‑minute reminder
            appointment.Reminders.Add(AppointmentReminder.Default15MinReminder);

            // Verify that the reminder was added
            if (appointment.Reminders.Count > 0)
            {
                Console.WriteLine("Reminder added. Count: " + appointment.Reminders.Count);
                AppointmentReminder reminder = appointment.Reminders[0];
                Console.WriteLine("Reminder Trigger: " + reminder.Trigger);
            }
            else
            {
                Console.WriteLine("No reminder was added.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
