using System;
using System.IO;
using Aspose.Email;
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
                // Input iCalendar file and output MSG file paths
                string icsFilePath = "input.ics";
                string msgFilePath = "output.msg";

                // Verify that the source .ics file exists
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {icsFilePath}");
                    return;
                }

                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(icsFilePath);

                // Convert the Appointment to a MAPI message preserving all properties
                MapiMessage mapiMessage = appointment.ToMapiMessage();

                // Save the MAPI message as an Outlook MSG file
                mapiMessage.Save(msgFilePath);
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
