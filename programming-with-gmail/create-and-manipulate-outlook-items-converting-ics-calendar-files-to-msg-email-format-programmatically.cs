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
            // Define input and output paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Guard input file existence
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating directory: {ex.Message}");
                    return;
                }
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading iCalendar file: {ex.Message}");
                return;
            }

            // Convert the Appointment to a MAPI message
            MapiMessage mapiMessage = appointment.ToMapiMessage();

            // Save the MAPI message as an Outlook MSG file
            try
            {
                mapiMessage.Save(msgPath);
                Console.WriteLine($"Successfully converted '{icsPath}' to '{msgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
            }
            finally
            {
                // Dispose the MAPI message
                if (mapiMessage != null)
                {
                    mapiMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
