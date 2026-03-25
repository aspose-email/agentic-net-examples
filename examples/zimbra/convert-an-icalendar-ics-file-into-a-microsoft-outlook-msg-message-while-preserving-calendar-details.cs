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

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    string inputDir = Path.GetDirectoryName(inputPath);
                    if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                    {
                        Directory.CreateDirectory(inputDir);
                    }

                    using (StreamWriter writer = new StreamWriter(inputPath))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("BEGIN:VEVENT");
                        writer.WriteLine("DTSTART:20230101T090000Z");
                        writer.WriteLine("DTEND:20230101T100000Z");
                        writer.WriteLine("SUMMARY:Placeholder Event");
                        writer.WriteLine("END:VEVENT");
                        writer.WriteLine("END:VCALENDAR");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
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
            Aspose.Email.Calendar.Appointment appointment;
            try
            {
                appointment = Aspose.Email.Calendar.Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message
            Aspose.Email.Mapi.MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as a .msg file
            try
            {
                using (Aspose.Email.Mapi.MapiMessage msg = mapiMessage)
                {
                    msg.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
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