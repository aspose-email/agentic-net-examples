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
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
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
                Console.Error.WriteLine($"Failed to load .ics file: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message and save as .msg
            try
            {
                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert and save .msg file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}