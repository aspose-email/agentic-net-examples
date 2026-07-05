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
            // Author note: Example demonstrates loading an iCalendar file, updating its location, and saving as MSG.
            string icsPath = "event.ics";
            string msgPath = "updated_event.msg";

            // Verify input file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
            }

            // Load the appointment from the .ics file
            Appointment appointment = Appointment.Load(icsPath);

            // Modify the location of the appointment
            appointment.Location = "New Location";

            // Convert the appointment to a MAPI message
            MapiMessage mapiMessage = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file
            mapiMessage.Save(msgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
