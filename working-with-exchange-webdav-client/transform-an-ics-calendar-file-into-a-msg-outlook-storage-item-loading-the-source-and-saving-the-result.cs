using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.ics";
            string outputPath = "output.msg";

            // Ensure input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a minimal placeholder .ics file
                    File.WriteAllText(inputPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the .ics file as an Appointment
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load .ics file: {ex.Message}");
                return;
            }

            // Convert to MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert or save message: {ex.Message}");
                return;
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}