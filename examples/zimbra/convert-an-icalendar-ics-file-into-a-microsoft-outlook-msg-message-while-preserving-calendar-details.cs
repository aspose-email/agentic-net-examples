using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace CalendarToMsgExample
{
    class Program
    {
        static void Main(string[] args)
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
                string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(inputIcsPath);

                // Convert the Appointment to a MAPI message
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MAPI message as a .msg file
                    mapiMessage.Save(outputMsgPath);
                }

                Console.WriteLine($"Successfully converted '{inputIcsPath}' to '{outputMsgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}