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
            // Path to the input iCalendar file
            string icsPath = "sample.ics";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nDTSTART:20230101T090000Z\r\nDTEND:20230101T100000Z\r\nSUMMARY:Sample Event\r\nEND:VEVENT\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholderIcs);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder iCalendar file – {ex.Message}");
                    return;
                }
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading iCalendar file – {ex.Message}");
                return;
            }

            // Convert the Appointment to a MAPI message (MSG)
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting appointment to MAPI message – {ex.Message}");
                return;
            }

            // Save the MSG file
            string msgPath = Path.ChangeExtension(icsPath, ".msg");
            try
            {
                using (MapiMessage message = mapiMessage)
                {
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving MSG file – {ex.Message}");
                return;
            }

            Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
