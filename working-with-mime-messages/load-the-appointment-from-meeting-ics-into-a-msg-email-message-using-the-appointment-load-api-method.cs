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
            string inputPath = "meeting.ics";
            string outputPath = "meeting.msg";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20230101T000000Z\r\nDTSTART:20230101T090000Z\r\nDTEND:20230101T100000Z\r\nSUMMARY:Placeholder Meeting\r\nEND:VEVENT\r\nEND:VCALENDAR";
                try
                {
                    File.WriteAllText(inputPath, placeholderIcs);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
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

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message
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

            // Save the MAPI message as a .msg file
            try
            {
                using (mapMessage)
                {
                    mapMessage.Save(outputPath);
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
