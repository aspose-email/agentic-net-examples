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
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                string placeholder = "BEGIN:VCALENDAR\r\nEND:VCALENDAR";
                File.WriteAllText(icsPath, placeholder);
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the iCalendar file, convert to MAPI message, and save as .msg
            using (FileStream icsStream = new FileStream(icsPath, FileMode.Open, FileAccess.Read))
            {
                Appointment appointment = Appointment.Load(icsStream);
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}