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
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));
            attendees.Add(new MailAddress("person3@example.com"));

            // Organizer address
            MailAddress organizer = new MailAddress("organizer@example.com");

            // Create the appointment using the constructor that accepts location, summary, description, dates, organizer and attendees
            Appointment appointment = new Appointment(
                "Conference Room",
                "Project Kickoff",
                "Discuss project goals and milestones.",
                new DateTime(2023, 10, 1, 9, 0, 0),
                new DateTime(2023, 10, 1, 10, 0, 0),
                organizer,
                attendees
            );

            // Configure additional required properties
            appointment.Location = "Conference Room A";
            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Initial meeting to discuss project scope and deliverables.";
            appointment.StartDate = new DateTime(2023, 10, 1, 9, 0, 0);
            appointment.EndDate = new DateTime(2023, 10, 1, 10, 0, 0);
            appointment.Organizer = organizer;
            appointment.Attendees = attendees;

            Console.WriteLine("Appointment created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
