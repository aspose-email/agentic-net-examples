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
            string icsPath = "appointment.ics";
            string msgPath = "appointment.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing.
            if (!File.Exists(icsPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(icsPath))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("END:VCALENDAR");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file.
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

            // Convert the appointment to a MAPI message, preserving attendees.
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MAPI message as a .msg file.
                    try
                    {
                        mapiMessage.Save(msgPath);
                        Console.WriteLine($"MSG file saved to {msgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion to MAPI message failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
