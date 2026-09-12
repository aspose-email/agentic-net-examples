using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input iCalendar file and output MSG file paths
                string icsPath = "meeting.ics";
                string msgPath = "meeting.msg";

                // Verify the input file exists before proceeding
                if (!File.Exists(icsPath))
                {
                    Console.Error.WriteLine($"Input file not found: {icsPath}");
                    return;
                }

                // Load the appointment from the .ics file
                Appointment appointment = Appointment.Load(icsPath);

                // Convert the appointment to a MAPI message
                MapiMessage mapMessage = appointment.ToMapiMessage();

                // Save the MAPI message as a .msg file
                mapMessage.Save(msgPath);

                Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
            }
            catch (Exception ex)
            {
                // Gracefully report any errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
