using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace AsposeEmailIcsToMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output file paths
                string icsFilePath = "sample.ics";
                string msgFilePath = "output.msg";

                // Verify that the input .ics file exists
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file '{icsFilePath}' does not exist.");
                    return;
                }

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(msgFilePath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the appointment from the .ics file
                Appointment appointment = Appointment.Load(icsFilePath);

                // Convert the appointment to a MAPI message and save as .msg
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgFilePath);
                }

                Console.WriteLine($"Successfully converted '{icsFilePath}' to '{msgFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}