using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for input iCalendar file and output Outlook MSG file
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(icsPath))
            {
                string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                File.WriteAllText(icsPath, placeholderIcs);
                Console.WriteLine($"Placeholder iCalendar file created at '{icsPath}'.");
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            using (FileStream icsStream = File.OpenRead(icsPath))
            {
                appointment = Appointment.Load(icsStream);
            }

            // Convert the appointment to a MAPI message
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                // Save the MAPI message as a .msg file
                mapiMessage.Save(msgPath);
            }

            Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}