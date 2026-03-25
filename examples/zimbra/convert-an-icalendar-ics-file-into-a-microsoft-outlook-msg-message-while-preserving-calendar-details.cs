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
            string icsFilePath = "input.ics";
            string msgFilePath = "output.msg";

            // Verify input file exists
            if (!File.Exists(icsFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {icsFilePath}");
                return;
            }

            try
            {
                // Load the iCalendar file
                Appointment appointment = Appointment.Load(icsFilePath);

                // Convert to MAPI message
                MapiMessage mapiMessage = appointment.ToMapiMessage();

                // Save as Outlook MSG
                using (mapiMessage)
                {
                    mapiMessage.Save(msgFilePath);
                }

                Console.WriteLine($"Successfully converted '{icsFilePath}' to '{msgFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}