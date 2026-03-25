using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input iCalendar file path
            string inputPath = "input.ics";
            // Output Outlook MSG file path
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine("Input file not found: " + inputPath);
                return;
            }

            // Ensure output directory exists
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
                Console.Error.WriteLine("Failed to load appointment: " + ex.Message);
                return;
            }

            // Convert the appointment to a MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to convert or save MSG: " + ex.Message);
                return;
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}