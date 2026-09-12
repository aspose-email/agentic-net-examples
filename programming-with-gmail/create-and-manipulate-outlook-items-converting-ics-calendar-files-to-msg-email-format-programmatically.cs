using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - Convert iCalendar (.ics) to Outlook Message (.msg)
class Program
{
    static void Main()
    {
        try
        {
            // Input iCalendar file path
            string icsPath = "event.ics";
            // Output MSG file path
            string msgPath = "event.msg";

            // Verify input file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment = Appointment.Load(icsPath);

            // Convert the Appointment to a MAPI message
            MapiMessage mapMessage = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file
            mapMessage.Save(msgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
