using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace IcsToMsgConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string icsFilePath = "calendar.ics";
                string msgFilePath = "calendar.msg";

                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {icsFilePath}");
                    return;
                }

                string outputDirectory = Path.GetDirectoryName(msgFilePath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                Appointment appointment = Appointment.Load(icsFilePath);
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
