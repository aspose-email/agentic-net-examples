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

            // Ensure the input .ics file exists
            if (!File.Exists(icsPath))
            {
                try
                {
                    // Create a minimal placeholder .ics file
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
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
                Console.Error.WriteLine($"Failed to load appointment from .ics: {ex.Message}");
                return;
            }

            // Modify the location
            appointment.Location = "New Conference Room";

            // Convert to MAPI message
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save as Outlook .msg file
            try
            {
                using (mapiMessage)
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save .msg file: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment exported to MSG successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
