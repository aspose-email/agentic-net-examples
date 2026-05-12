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
            // Path to the .ics file
            string icsPath = "appointment.ics";

            // Ensure the file exists; create a minimal placeholder if it does not
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nDTSTART:20240101T090000Z\r\nDTEND:20240101T100000Z\r\nSUMMARY:Placeholder Event\r\nEND:VEVENT\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
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

            // Extract start and end timestamps
            DateTime startDate = appointment.StartDate;
            DateTime endDate = appointment.EndDate;

            Console.WriteLine($"Start: {startDate:O}");
            Console.WriteLine($"End:   {endDate:O}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
