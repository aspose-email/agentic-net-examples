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

            // Verify the .ics file exists before attempting to load it
            if (!File.Exists(icsFilePath))
            {
                Console.Error.WriteLine($"File not found: {icsFilePath}");
                return;
            }

            Appointment appointment = null;
            try
            {
                // Load the appointment from the .ics file
                appointment = Appointment.Load(icsFilePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load appointment: {loadEx.Message}");
                return;
            }

            // Output the plain‑text description of the appointment
            Console.WriteLine("Appointment Description:");
            Console.WriteLine(appointment.Description ?? "(no description)");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
