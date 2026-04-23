using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Create an appointment
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 15, 14, 0, 0),
                new DateTime(2023, 12, 15, 15, 0, 0),
                organizer,
                attendees);

            appointment.Summary = "Project Review Meeting";
            appointment.Description = "Discuss project milestones and next steps.";

            // Set a reminder to trigger 10 minutes before the start time
            AppointmentReminder reminder = new AppointmentReminder();
            reminder.Trigger = new ReminderTrigger(
                new ReminderDuration(TimeSpan.FromMinutes(10)),
                ReminderRelated.Start);
            appointment.Reminders.Add(reminder);

            // Output details to console
            Console.WriteLine("Appointment created with a reminder 10 minutes before start.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
