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
            // Define input and output file paths
            string inputIcsPath = "input.ics";
            string outputMsgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputIcsPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputIcsPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment = Appointment.Load(inputIcsPath);

            // Convert the Appointment to a MAPI message
            MapiMessage mapMsg = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file
            mapMsg.Save(outputMsgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
