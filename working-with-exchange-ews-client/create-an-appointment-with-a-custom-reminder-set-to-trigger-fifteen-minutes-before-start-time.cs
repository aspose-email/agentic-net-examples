using System;
using Aspose.Email;
using Aspose.Email.Calendar;

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

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 25, 10, 0, 0),
                new DateTime(2023, 12, 25, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timeline.";

            // Add a 15‑minute reminder
            AppointmentReminder reminder = AppointmentReminder.Default15MinReminder;
            appointment.Reminders.Add(reminder);

            Console.WriteLine($"Reminder added: {appointment.Reminders.Count} reminder(s).");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
