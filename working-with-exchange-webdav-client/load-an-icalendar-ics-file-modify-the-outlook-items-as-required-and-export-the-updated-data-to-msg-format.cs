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

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                File.WriteAllText(inputPath, placeholderIcs);
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the iCalendar file
            Appointment appointment = Appointment.Load(inputPath);

            // Modify the appointment as required (example: change summary)
            appointment.Summary = "Updated Summary";

            // Convert to MSG (MAPI) format and save
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                mapiMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
