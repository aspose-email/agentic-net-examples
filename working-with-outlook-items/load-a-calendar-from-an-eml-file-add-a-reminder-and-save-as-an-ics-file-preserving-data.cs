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
            string emlPath = "input.eml";
            string icsPath = "output.ics";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file '{emlPath}' not found.");
                return;
            }

            // Load the appointment from the EML file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Add a reminder (default reminder)
            try
            {
                appointment.Reminders.Add(new AppointmentReminder());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add reminder: {ex.Message}");
                // Continue without reminder if adding fails
            }

            // Save the appointment as an iCalendar (ICS) file
            try
            {
                appointment.Save(icsPath);
                Console.WriteLine($"Appointment saved to '{icsPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
