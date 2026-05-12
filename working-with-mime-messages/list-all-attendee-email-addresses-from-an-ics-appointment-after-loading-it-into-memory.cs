using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string icsPath = "appointment.ics";

            if (!File.Exists(icsPath))
            {
                string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTART:20230101T090000Z\r\nDTEND:20230101T100000Z\r\nSUMMARY:Placeholder Event\r\nATTENDEE:mailto:placeholder@example.com\r\nEND:VEVENT\r\nEND:VCALENDAR";
                File.WriteAllText(icsPath, placeholderIcs);
            }

            Appointment appointment = Appointment.Load(icsPath);

            MailAddressCollection attendees = appointment.Attendees;
            Console.WriteLine("Attendees:");
            foreach (MailAddress address in attendees)
            {
                Console.WriteLine(address.Address);
            }

            MailAddressCollection optionalAttendees = appointment.OptionalAttendees;
            if (optionalAttendees != null && optionalAttendees.Count > 0)
            {
                Console.WriteLine("Optional Attendees:");
                foreach (MailAddress opt in optionalAttendees)
                {
                    Console.WriteLine(opt.Address);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
