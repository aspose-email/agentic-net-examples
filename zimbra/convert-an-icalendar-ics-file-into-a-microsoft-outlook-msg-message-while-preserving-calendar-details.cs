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
    try
    {
        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Placeholder//EN\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20260101T000000Z\r\nDTSTART:20260101T000000Z\r\nDTEND:20260101T010000Z\r\nSUMMARY:Placeholder\r\nEND:VEVENT\r\nEND:VCALENDAR";
        File.WriteAllText(icsFilePath, placeholderIcs);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder ICS: {ex.Message}");
        return;
    }
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
