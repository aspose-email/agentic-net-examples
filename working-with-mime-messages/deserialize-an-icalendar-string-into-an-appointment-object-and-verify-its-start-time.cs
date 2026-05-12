using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Sample iCalendar content representing a single event
            string icsContent = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nUID:12345\r\nDTSTART:20230101T100000Z\r\nDTEND:20230101T110000Z\r\nSUMMARY:Sample Event\r\nEND:VEVENT\r\nEND:VCALENDAR";

            // Convert the string to a UTF‑8 byte array
            byte[] icsBytes = Encoding.UTF8.GetBytes(icsContent);

            // Load the appointment from the memory stream
            using (MemoryStream memoryStream = new MemoryStream(icsBytes))
            {
                Appointment appointment = Appointment.Load(memoryStream);

                // Expected start time (UTC) matching the DTSTART above
                DateTime expectedStart = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc);

                if (appointment.StartDate == expectedStart)
                {
                    Console.WriteLine("Start time matches the expected value.");
                }
                else
                {
                    Console.WriteLine($"Start time mismatch. Expected: {expectedStart:o}, Actual: {appointment.StartDate:o}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
