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
            // Input and output file paths
            const string icsPath = "input.ics";
            const string msgPath = "output.msg";

            // Verify the .ics file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($".ics file not found: {icsPath}");
                return;
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                // Appointment.Load is the standard way to read an iCalendar file
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Modify the location of the appointment
            appointment.Location = "New Conference Room";

            // Convert the appointment to a MAPI message
            MapiMessage mapMsg = appointment.ToMapiMessage();

            // Save the MAPI message as an Outlook .msg file
            try
            {
                mapMsg.Save(msgPath);
                Console.WriteLine($"Appointment exported to MSG: {msgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
