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
            string icsPath = "appointment.ics";

            // Ensure the file exists; create a minimal placeholder if it does not.
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholder);
                    Console.WriteLine($"Placeholder iCalendar file created at '{icsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the iCalendar file.
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Retrieve and display the iCalendar version.
            string version = appointment.Version;
            Console.WriteLine($"iCalendar version: {version}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
