using System;

namespace AppointmentSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create attendees collection
                Aspose.Email.MailAddressCollection attendees = new Aspose.Email.MailAddressCollection();
                attendees.Add(new Aspose.Email.MailAddress("alice@example.com"));
                attendees.Add(new Aspose.Email.MailAddress("bob@example.com"));

                // Organizer address
                Aspose.Email.MailAddress organizer = new Aspose.Email.MailAddress("organizer@example.com");

                // Create appointment using the constructor with location, summary, description, dates, organizer, and attendees
                Aspose.Email.Calendar.Appointment appointment = new Aspose.Email.Calendar.Appointment(
                    "Conference Room 1",
                    "Project Kickoff",
                    "Discuss project goals and milestones.",
                    new DateTime(2023, 10, 15, 10, 0, 0),
                    new DateTime(2023, 10, 15, 11, 0, 0),
                    organizer,
                    attendees);

                // Additional property configuration (optional, already set via constructor)
                appointment.Location = "Conference Room 1";
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and milestones.";
                appointment.StartDate = new DateTime(2023, 10, 15, 10, 0, 0);
                appointment.EndDate = new DateTime(2023, 10, 15, 11, 0, 0);
                appointment.Organizer = organizer;
                appointment.Attendees = attendees;

                // Output appointment details
                Console.WriteLine("Appointment created:");
                Console.WriteLine("Summary: " + appointment.Summary);
                Console.WriteLine("Location: " + appointment.Location);
                Console.WriteLine("Start: " + appointment.StartDate);
                Console.WriteLine("End: " + appointment.EndDate);
                Console.WriteLine("Organizer: " + appointment.Organizer.Address);
                Console.WriteLine("Attendees:");
                foreach (Aspose.Email.MailAddress address in appointment.Attendees)
                {
                    Console.WriteLine(" - " + address.Address);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}