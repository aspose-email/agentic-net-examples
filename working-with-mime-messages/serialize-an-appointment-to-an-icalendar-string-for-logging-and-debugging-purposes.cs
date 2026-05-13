using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Room 112",
                new DateTime(2023, 10, 1, 9, 0, 0),
                new DateTime(2023, 10, 1, 10, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Team Sync";
            appointment.Description = "Weekly team sync meeting.";

            // Serialize to iCalendar string using a memory stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                appointment.Save(memoryStream, AppointmentSaveFormat.Ics);
                memoryStream.Position = 0;
                using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    string icalString = reader.ReadToEnd();
                    Console.WriteLine("iCalendar representation:");
                    Console.WriteLine(icalString);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
