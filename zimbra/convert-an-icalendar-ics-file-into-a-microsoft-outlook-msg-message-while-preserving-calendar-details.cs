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

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    string placeholderIcs = "BEGIN:VCALENDAR\r\nEND:VCALENDAR";
                    File.WriteAllText(inputPath, placeholderIcs);
                    Console.WriteLine($"Placeholder iCalendar file created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                return;
            }

            // Convert the Appointment to a MAPI message
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert Appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as a .msg file
            try
            {
                using (MapiMessage messageToSave = mapiMessage)
                {
                    messageToSave.Save(outputPath);
                }
                Console.WriteLine($"Successfully saved MSG file to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
