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
            string icsFilePath = "input.ics";
            string msgFilePath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing.
            if (!File.Exists(icsFilePath))
            {
                try
                {
                    string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                    File.WriteAllText(icsFilePath, placeholderIcs);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(msgFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file.
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load .ics file: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message.
            MapiMessage mapMessage;
            try
            {
                mapMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as a .msg file.
            try
            {
                using (mapMessage)
                {
                    mapMessage.Save(msgFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save .msg file: {ex.Message}");
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
