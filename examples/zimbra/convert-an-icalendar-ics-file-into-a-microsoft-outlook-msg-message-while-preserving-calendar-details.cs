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

            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file '{icsPath}' does not exist.");
                return;
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment = Appointment.Load(icsPath);

            // Convert the Appointment to a MAPI message and save as .msg
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
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