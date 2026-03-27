using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // Define calendar identifier (e.g., "primary")
            string calendarId = "primary";

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));
            attendees.Add(new MailAddress("person3@example.com"));

            // Create appointment
            Appointment appointment = new Appointment(
                "Conference Room A",
                new DateTime(2024, 5, 20, 10, 0, 0),
                new DateTime(2024, 5, 20, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timelines.";

            // Add the appointment to the specified calendar
            Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
            Console.WriteLine("Appointment created with ID: " + created.UniqueId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
