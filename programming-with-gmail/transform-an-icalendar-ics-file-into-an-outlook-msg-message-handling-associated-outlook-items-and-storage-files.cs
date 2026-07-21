using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace IcsToMsgConverter
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input iCalendar file path
                string icsFilePath = "sample.ics";
                // Output Outlook MSG file path
                string msgFilePath = "output.msg";

                // Verify input file exists
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {icsFilePath}");
                    return;
                }

                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(icsFilePath);

                // Convert the Appointment to a MAPI message
                MapiMessage mapMessage = appointment.ToMapiMessage();

                // Save the MAPI message as an Outlook MSG file
                mapMessage.Save(msgFilePath);

                Console.WriteLine($"Successfully converted '{icsFilePath}' to '{msgFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
