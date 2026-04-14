using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Simulated database query result set containing attendee email addresses
            string[] attendeeEmails = new string[] { "alice@example.com", "bob@example.com", "carol@example.com" };

            // Build the collection of attendees required by the Appointment constructor
            MailAddressCollection attendees = new MailAddressCollection();
            foreach (string email in attendeeEmails)
            {
                attendees.Add(new MailAddress(email));
            }

            // Create the appointment with location, start/end times, organizer, and required attendees
            Appointment appointment = new Appointment(
                "Conference Room 1",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timeline.";

            // Output appointment details to the console
            Console.WriteLine("Appointment created:");
            Console.WriteLine("Location: " + appointment.Location);
            Console.WriteLine("Start: " + appointment.StartDate);
            Console.WriteLine("End: " + appointment.EndDate);
            Console.WriteLine("Organizer: " + appointment.Organizer);
            Console.WriteLine("Attendees:");
            foreach (MailAddress address in appointment.Attendees)
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
