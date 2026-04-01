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
            string inputIcsPath = "sample.ics";
            string outputMsgPath = "output.msg";

            // Verify that the input .ics file exists
            if (!File.Exists(inputIcsPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputIcsPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputIcsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to load iCalendar – {ex.Message}");
                return;
            }

            // Convert the Appointment to a MAPI message
            MapiMessage mapMsg;
            try
            {
                mapMsg = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Conversion to MAPI message failed – {ex.Message}");
                return;
            }

            // Save the MAPI message as an Outlook MSG file
            try
            {
                using (mapMsg)
                {
                    mapMsg.Save(outputMsgPath);
                }
                Console.WriteLine($"MSG file created successfully at {outputMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to save MSG file – {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
