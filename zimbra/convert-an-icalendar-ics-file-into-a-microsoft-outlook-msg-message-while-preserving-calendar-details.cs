using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                    Console.WriteLine($"Placeholder iCalendar file created at '{icsPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ioEx.Message}");
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
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Aspose.Email.Calendar.Appointment appointment;
            try
            {
                appointment = Aspose.Email.Calendar.Appointment.Load(icsPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {loadEx.Message}");
                return;
            }

            // Convert to MAPI message and save as .msg
            try
            {
                using (Aspose.Email.Mapi.MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
                Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
            }
            catch (Exception convertEx)
            {
                Console.Error.WriteLine($"Conversion failed: {convertEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}