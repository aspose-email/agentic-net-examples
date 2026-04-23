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
                    using (StreamWriter writer = new StreamWriter(icsPath))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("BEGIN:VEVENT");
                        writer.WriteLine("UID:placeholder");
                        writer.WriteLine("DTSTAMP:20240101T000000Z");
                        writer.WriteLine("DTSTART:20240102T100000Z");
                        writer.WriteLine("DTEND:20240102T110000Z");
                        writer.WriteLine("SUMMARY:Sample Event");
                        writer.WriteLine("LOCATION:Old Location");
                        writer.WriteLine("END:VEVENT");
                        writer.WriteLine("END:VCALENDAR");
                    }
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
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Modify the location
            appointment.Location = "New Location";

            // Convert to MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(msgPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert/save MSG: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment location updated and saved as MSG successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
