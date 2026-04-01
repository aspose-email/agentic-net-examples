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
            string inputIcsPath = "input.ics";
            string outputMsgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputIcsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputIcsPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                    return;
                }
            }

            try
            {
                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(inputIcsPath);

                // Convert the Appointment to a MAPI message
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MAPI message as a MSG file
                    mapiMessage.Save(outputMsgPath);
                }

                Console.WriteLine($"Successfully converted '{inputIcsPath}' to '{outputMsgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unexpected error: {outerEx.Message}");
        }
    }
}
