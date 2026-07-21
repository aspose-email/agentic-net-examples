// Author: Aspose.Email example - creates an Appointment and sets its properties
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
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment using the constructor with location, summary, description, dates, organizer and attendees
            Appointment appointment = new Appointment(
                "Conference Room 1",
                "Project Kickoff",
                "Discuss project goals and milestones.",
                new DateTime(2024, 10, 1, 10, 0, 0),
                new DateTime(2024, 10, 1, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Configure additional required properties
            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Initial meeting to outline project scope and deliverables.";
            appointment.Location = "Conference Room 1";

            // Display the created appointment details
            Console.WriteLine("Appointment created:");
            Console.WriteLine($"Location: {appointment.Location}");
            Console.WriteLine($"Summary: {appointment.Summary}");
            Console.WriteLine($"Description: {appointment.Description}");
            Console.WriteLine($"Start: {appointment.StartDate}");
            Console.WriteLine($"End: {appointment.EndDate}");
            Console.WriteLine($"Organizer: {appointment.Organizer}");
            Console.WriteLine($"Attendees count: {appointment.Attendees.Count}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
