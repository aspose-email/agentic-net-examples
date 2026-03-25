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
            // Paths for input iCalendar file and output MSG file
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Verify that the input file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            using (FileStream icsStream = new FileStream(icsPath, FileMode.Open, FileAccess.Read))
            {
                appointment = Appointment.Load(icsStream);
            }

            // Convert the appointment to a MAPI message
            MapiMessage mapiMessage = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file
            mapiMessage.Save(msgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}