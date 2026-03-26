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
            string icsPath = "calendar.ics";
            string msgPath = "message.msg";

            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            try
            {
                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(icsPath);

                // Convert the Appointment to a MAPI message
                MapiMessage mapiMessage = appointment.ToMapiMessage();

                // Save the MAPI message as an Outlook MSG file
                using (mapiMessage)
                {
                    mapiMessage.Save(msgPath);
                }

                Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}