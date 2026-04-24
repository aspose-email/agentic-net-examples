using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));
            attendees.Add(new MailAddress("person3@example.com"));

            // Create an appointment with location, summary, description, start/end times, organizer, and attendees
            Appointment appointment = new Appointment(
                "Conference Room 1",
                "Team Sync Meeting",
                "Discuss project milestones and next steps.",
                new DateTime(2023, 10, 1, 9, 0, 0),
                new DateTime(2023, 10, 1, 10, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees
            );

            // Output appointment details
            Console.WriteLine("Location: " + appointment.Location);
            Console.WriteLine("Start: " + appointment.StartDate);
            Console.WriteLine("End: " + appointment.EndDate);
            Console.WriteLine("Summary: " + appointment.Summary);
            Console.WriteLine("Description: " + appointment.Description);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
