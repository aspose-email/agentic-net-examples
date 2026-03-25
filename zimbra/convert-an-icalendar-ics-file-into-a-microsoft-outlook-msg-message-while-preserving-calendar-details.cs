using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace CalendarToMsgConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string icsPath = "sample.ics";
                string msgPath = "output.msg";

                // Ensure the input .ics file exists
                if (!File.Exists(icsPath))
                {
                    try
                    {
                        File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
                        Console.WriteLine($"Created placeholder iCalendar file at '{icsPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
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
                    Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                    return;
                }

                // Convert the appointment to a MAPI message
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

                // Save the MAPI message as a .msg file
                try
                {
                    using (MapiMessage disposableMessage = mapiMessage)
                    {
                        disposableMessage.Save(msgPath);
                    }
                    Console.WriteLine($"MSG file saved to '{msgPath}'.");
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
}