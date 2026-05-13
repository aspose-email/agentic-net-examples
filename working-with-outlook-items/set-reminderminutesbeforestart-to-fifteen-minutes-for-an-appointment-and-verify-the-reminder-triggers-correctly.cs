using System;
using System.IO;
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

            // Create an appointment
            Appointment appointment = new Appointment(
                location: "Conference Room",
                startDate: new DateTime(2023, 12, 25, 10, 0, 0),
                endDate: new DateTime(2023, 12, 25, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            appointment.Summary = "Project Review";
            appointment.Description = "Discuss project milestones.";

            // Add a 15‑minute reminder (default reminder)
            appointment.Reminders.Add(AppointmentReminder.Default15MinReminder);

            // Verify that the reminder was added
            if (appointment.Reminders.Count > 0)
            {
                Console.WriteLine("Reminder added successfully. Total reminders: " + appointment.Reminders.Count);
            }
            else
            {
                Console.WriteLine("Failed to add reminder.");
                return;
            }

            // Save the appointment to an iCalendar file
            string outputPath = "appointment.ics";

            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                appointment.Save(outputPath);
                Console.WriteLine("Appointment saved to: " + Path.GetFullPath(outputPath));
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("File I/O error: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
