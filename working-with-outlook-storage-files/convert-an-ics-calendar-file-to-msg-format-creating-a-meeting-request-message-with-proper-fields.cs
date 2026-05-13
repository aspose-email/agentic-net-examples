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
                Console.Error.WriteLine($"Input file not found: {inputIcsPath}");
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
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputIcsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI meeting request message and save as .msg
            try
            {
                using (MapiMessage meetingMessage = appointment.ToMapiMessage())
                {
                    meetingMessage.Save(outputMsgPath);
                }
                Console.WriteLine($"MSG file created successfully at {outputMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert or save MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
