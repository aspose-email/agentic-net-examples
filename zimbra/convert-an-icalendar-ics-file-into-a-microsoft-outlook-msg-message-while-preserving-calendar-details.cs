using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.ics";
            string outputPath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                File.WriteAllText(inputPath, placeholderIcs);
                Console.WriteLine($"Placeholder iCalendar file created at '{inputPath}'.");
            }

            // Load the appointment from the .ics file.
            Appointment appointment = Appointment.Load(inputPath);

            // Convert the appointment to a MAPI message and save as .msg.
            using (MapiMessage msg = appointment.ToMapiMessage())
            {
                msg.Save(outputPath);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
