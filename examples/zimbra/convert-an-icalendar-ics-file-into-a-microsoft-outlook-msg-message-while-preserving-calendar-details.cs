using System;
using System.IO;
using Aspose.Email.Calendar;

namespace AsposeEmailConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string icsFilePath = "sample.ics";
                string msgFilePath = "output.msg";

                // Verify that the source .ics file exists
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {icsFilePath}");
                    return;
                }

                try
                {
                    // Load the iCalendar file into an Appointment object
                    Appointment appointment = Appointment.Load(icsFilePath);

                    // Save the appointment as a .msg file preserving calendar details
                    appointment.Save(msgFilePath, AppointmentSaveFormat.Msg);

                    Console.WriteLine($"Conversion successful. MSG saved to: {msgFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}