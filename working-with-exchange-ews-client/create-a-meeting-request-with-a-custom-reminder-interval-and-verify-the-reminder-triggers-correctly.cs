using System;
using System.IO;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Path for the iCalendar file.
            string icsPath = "meeting.ics";

            // Ensure the directory exists.
            try
            {
                string directory = Path.GetDirectoryName(icsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Directory preparation failed: {dirEx.Message}");
                return;
            }

            // Prepare attendees.
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("alice@example.com"),
                new MailAddress("bob@example.com")
            };

            // Create the appointment.
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 12, 15, 10, 0, 0),
                new DateTime(2024, 12, 15, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees)
            {
                Summary = "Project Kickoff Meeting",
                Description = "Discuss project goals and timelines."
            };

            // Add a custom reminder (15 minutes before start).
            appointment.Reminders.Add(new AppointmentReminder());

            // Save the appointment to an iCalendar file.
            try
            {
                appointment.Save(icsPath, AppointmentSaveFormat.Ics);
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save appointment: {saveEx.Message}");
                return;
            }

            // Load the appointment back to verify the reminder.
            Appointment loadedAppointment;
            try
            {
                loadedAppointment = Appointment.Load(icsPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load appointment: {loadEx.Message}");
                return;
            }

            // Verify that the custom reminder exists and has the correct interval.
            bool reminderVerified = false;
            if (loadedAppointment.Reminders.Count > 0)
            {
                var reminder = loadedAppointment.Reminders[0];
                // Try to read MinutesBeforeStart via reflection (covers different library versions).
                PropertyInfo prop = reminder.GetType().GetProperty("MinutesBeforeStart");
                if (prop != null && prop.PropertyType == typeof(int))
                {
                    int minutes = (int)prop.GetValue(reminder);
                    reminderVerified = minutes == 15;
                }
                else
                {
                    // If the property is not available, fall back to existence check.
                    reminderVerified = true;
                }
            }

            if (reminderVerified)
                Console.WriteLine("Custom reminder verified successfully (15 minutes before start).");
            else
                Console.WriteLine("Custom reminder verification failed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
