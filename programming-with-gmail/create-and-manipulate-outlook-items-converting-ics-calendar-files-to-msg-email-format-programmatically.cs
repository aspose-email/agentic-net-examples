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
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            if (!File.Exists(icsPath))
{
    try
    {
        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Placeholder//EN\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20260101T000000Z\r\nDTSTART:20260101T000000Z\r\nDTEND:20260101T010000Z\r\nSUMMARY:Placeholder\r\nEND:VEVENT\r\nEND:VCALENDAR";
        File.WriteAllText(icsPath, placeholderIcs);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder ICS: {ex.Message}");
        return;
    }
}


            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading appointment: {loadEx.Message}");
                return;
            }

            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                try
                {
                    mapiMessage.Save(msgPath);
                    Console.WriteLine($"MSG file saved to {msgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving MSG: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
