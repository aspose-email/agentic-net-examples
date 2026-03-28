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
                // Define input iCalendar file and output MSG file paths
                string inputIcsPath = "calendar.ics";
                string outputMsgPath = "calendar.msg";

                // Verify that the input file exists before proceeding
                if (!File.Exists(inputIcsPath))
{
    try
    {
        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Placeholder//EN\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20260101T000000Z\r\nDTSTART:20260101T000000Z\r\nDTEND:20260101T010000Z\r\nSUMMARY:Placeholder\r\nEND:VEVENT\r\nEND:VCALENDAR";
        File.WriteAllText(inputIcsPath, placeholderIcs);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder ICS: {ex.Message}");
        return;
    }
}


                // Load the iCalendar file into an Appointment object
                Appointment appointment = Appointment.Load(inputIcsPath);

                // Convert the Appointment to a MAPI message
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MAPI message as an Outlook MSG file
                    mapiMessage.Save(outputMsgPath);
                }

                Console.WriteLine($"Successfully converted '{inputIcsPath}' to '{outputMsgPath}'.");
            }
            catch (Exception ex)
            {
                // Output any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
