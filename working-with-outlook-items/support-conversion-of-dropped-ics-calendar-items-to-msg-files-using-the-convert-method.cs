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
            // Paths for input .ics and output .msg files
            string inputPath = "sample.ics";
            string outputPath = "output.msg";

            // Verify the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading appointment: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as a .msg file
            try
            {
                using (MapiMessage messageToSave = mapiMessage)
                {
                    messageToSave.Save(outputPath);
                }
                Console.WriteLine($"Successfully saved MSG file to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
