using System;
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

            // Create an appointment using the constructor
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Configure required properties
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project scope and deliverables.";
            appointment.Location = "Conference Room";

            // Display appointment details
            Console.WriteLine("Appointment created:");
            Console.WriteLine($"Subject: {appointment.Summary}");
            Console.WriteLine($"Organizer: {appointment.Organizer}");
            Console.WriteLine($"Start: {appointment.StartDate}");
            Console.WriteLine($"End: {appointment.EndDate}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
