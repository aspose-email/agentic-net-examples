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
            // Initialize Gmail client with dummy credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                clientId: "clientId",
                clientSecret: "clientSecret",
                refreshToken: "refreshToken",
                defaultEmail: "user@example.com");

            // Prepare attendees list
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                location: "Conference Room",
                summary: "Project Sync",
                description: "Weekly project synchronization meeting.",
                startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                endDate: new DateTime(2024, 5, 20, 11, 0, 0),
                organizer: new MailAddress("organizer@domain.com"),
                attendees: attendees);

            // Add the appointment to the primary calendar
            Appointment created = gmailClient.CreateAppointment("primary", appointment);

            Console.WriteLine("Appointment created with UID: " + created.UniqueId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
