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
                string icsFilePath = "sample.ics";
                string msgFilePath = "output.msg";

                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {icsFilePath}");
                    return;
                }

                // Load the appointment from the .ics file
                Appointment appointment;
                using (FileStream icsStream = File.OpenRead(icsFilePath))
                {
                    appointment = Appointment.Load(icsStream);
                }

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