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
            // Input iCalendar file path
            string icsPath = "sample.ics";

            // Verify the input file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            // Output Outlook MSG file path
            string msgPath = "sample.msg";

            try
            {
                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(icsPath);

                // Convert the Appointment to a MapiMessage
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MapiMessage as an Outlook MSG file
                    mapiMessage.Save(msgPath);
                    Console.WriteLine($"MSG file saved to {msgPath}");
                }
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
