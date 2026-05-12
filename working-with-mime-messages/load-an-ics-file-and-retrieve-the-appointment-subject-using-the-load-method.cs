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
            string icsFilePath = "sample.ics";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(icsFilePath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(icsFilePath, false))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("END:VCALENDAR");
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ioEx.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment = null;
            try
            {
                appointment = Appointment.Load(icsFilePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load appointment: {loadEx.Message}");
                return;
            }

            // Retrieve and display the subject (Summary) of the appointment
            if (appointment != null)
            {
                Console.WriteLine($"Appointment Subject: {appointment.Summary}");
            }
            else
            {
                Console.WriteLine("No appointment loaded.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
