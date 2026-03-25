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
            string inputPath = "sample.ics";
            string outputPath = "appointment.msg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the iCalendar file
            Appointment appointment = Appointment.Load(inputPath);

            // Convert the appointment to a MAPI message
            MapiMessage mapiMessage = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file
            using (mapiMessage)
            {
                mapiMessage.Save(outputPath);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}