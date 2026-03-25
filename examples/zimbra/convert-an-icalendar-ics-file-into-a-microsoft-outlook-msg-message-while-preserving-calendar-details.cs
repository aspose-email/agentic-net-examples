using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input iCalendar file and output MSG file paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                    Console.WriteLine($"Placeholder iCalendar file created at '{icsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
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
                Console.Error.WriteLine($"Failed to load iCalendar file '{icsPath}': {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
                Console.WriteLine($"Successfully saved MSG file to '{msgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert or save MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}