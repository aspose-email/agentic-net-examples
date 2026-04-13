using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the iCalendar file containing the appointment
            string icsPath = "appointment.ics";

            // Guard against missing file
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"File not found: {icsPath}");
                return;
            }

            // Load the appointment from the .ics file
            Appointment appointment = Appointment.Load(icsPath);

            // Output basic appointment details
            Console.WriteLine($"Subject: {appointment.Summary}");
            Console.WriteLine($"Start : {appointment.StartDate}");
            Console.WriteLine($"End   : {appointment.EndDate}");

            // Output reminder settings
            if (appointment.Reminders != null && appointment.Reminders.Count > 0)
            {
                foreach (AppointmentReminder reminder in appointment.Reminders)
                {
                    // Reminder properties may include MinutesBeforeStart, ReminderAction, etc.
                    // Here we simply output the reminder object's string representation.
                    Console.WriteLine($"Reminder: {reminder}");
                }
            }
            else
            {
                Console.WriteLine("No reminders defined for this appointment.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
