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
            string icsPath = "input.ics";
            string msgPath = "output.msg";

            // Ensure input file exists; create minimal placeholder if missing
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
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(msgPath);
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
                appointment = Appointment.Load(icsPath);
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

            // Save the MAPI message as an Outlook MSG file
            try
            {
                using (MapiMessage disposableMessage = mapiMessage)
                {
                    disposableMessage.Save(msgPath);
                }
                Console.WriteLine($"MSG file saved successfully to '{msgPath}'.");
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
