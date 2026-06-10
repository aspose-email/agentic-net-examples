using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Appointment starts 5 minutes from now
            DateTime start = DateTime.Now.AddMinutes(5);
            DateTime end = start.AddHours(1);

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                start,
                end,
                organizer,
                attendees);
            appointment.Summary = "Team Sync";
            appointment.Description = "Discuss project updates.";

            // Create a reminder that triggers 5 minutes before the start (i.e., now)
            AppointmentReminder reminder = new AppointmentReminder();
            reminder.Trigger = new ReminderTrigger(start.AddMinutes(-5)); // equals DateTime.Now
            reminder.Summary = "Reminder: Meeting starts soon.";
            appointment.Reminders.Add(reminder);

            Console.WriteLine("Appointment created. Waiting for reminder...");

            // Simple test loop to detect when the reminder should fire
            bool reminderFired = false;
            while (!reminderFired)
            {
                if (DateTime.Now >= reminder.Trigger.DateTime)
                {
                    Console.WriteLine("Reminder fired: " + reminder.Summary);
                    reminderFired = true;
                }
                else
                {
                    // Sleep briefly to avoid tight loop
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
