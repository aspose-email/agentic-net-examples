using System;
using Aspose.Email;
using Aspose.Email.Calendar;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create a collection of attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));

                // Define the organizer
                MailAddress organizer = new MailAddress("organizer@example.com");

                // Create the appointment using the constructor with location, summary, description, dates, organizer and attendees
                Appointment appointment = new Appointment(
                    "Conference Room 1",
                    "Project Kickoff",
                    "Discuss project goals and timelines.",
                    new DateTime(2023, 10, 15, 10, 0, 0),
                    new DateTime(2023, 10, 15, 11, 0, 0),
                    organizer,
                    attendees);

                // Configure additional properties
                appointment.Location = "Conference Room 1";
                appointment.Summary = "Project Kickoff Meeting";
                appointment.Description = "Initial meeting to discuss project scope, deliverables, and schedule.";
                appointment.StartDate = new DateTime(2023, 10, 15, 10, 0, 0);
                appointment.EndDate = new DateTime(2023, 10, 15, 11, 0, 0);
                appointment.Organizer = organizer;
                appointment.Attendees = attendees;

                // Output appointment details
                Console.WriteLine("Appointment created:");
                Console.WriteLine($"Subject: {appointment.Summary}");
                Console.WriteLine($"Location: {appointment.Location}");
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
}
