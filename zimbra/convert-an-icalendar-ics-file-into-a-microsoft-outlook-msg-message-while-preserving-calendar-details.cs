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

                // Verify input file exists
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"Input file '{icsFilePath}' does not exist.");
                    return;
                }

                // Load the iCalendar file into an Appointment object
                Appointment appointment = null;
                try
                {
                    appointment = Appointment.Load(icsFilePath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load iCalendar file: {loadEx.Message}");
                    return;
                }

                // Convert the Appointment to a MapiMessage
                MapiMessage mapiMessage = null;
                try
                {
                    mapiMessage = appointment.ToMapiMessage();
                }
                catch (Exception convertEx)
                {
                    Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {convertEx.Message}");
                    return;
                }

                // Save the MapiMessage as a .msg file
                try
                {
                    using (MapiMessage messageToSave = mapiMessage)
                    {
                        messageToSave.Save(msgFilePath);
                    }
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}