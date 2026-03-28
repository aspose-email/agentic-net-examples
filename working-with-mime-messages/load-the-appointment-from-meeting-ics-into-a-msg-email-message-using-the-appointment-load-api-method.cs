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
            string icsPath = "meeting.ics";
            string msgPath = "meeting.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing.
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file.
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

            // Convert the appointment to a MAPI message.
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as a .msg file.
            try
            {
                // Ensure the directory for the output file exists.
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(msgPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (MapiMessage message = mapiMessage)
                {
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment successfully converted to MSG.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
