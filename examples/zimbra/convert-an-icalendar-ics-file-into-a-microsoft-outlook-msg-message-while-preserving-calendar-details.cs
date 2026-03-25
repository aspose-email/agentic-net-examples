using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string icsPath;
            string msgPath;

            if (args != null && args.Length >= 2)
            {
                icsPath = args[0];
                msgPath = args[1];
            }
            else
            {
                icsPath = "sample.ics";
                msgPath = "sample.msg";
            }

            // Verify input file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input iCalendar file not found: {icsPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {loadEx.Message}");
                return;
            }

            // Convert to MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
                Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
            }
            catch (Exception convertEx)
            {
                Console.Error.WriteLine($"Conversion failed: {convertEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}