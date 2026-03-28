using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with placeholder credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",          // Client ID
                "clientSecret",      // Client Secret
                "refreshToken",      // Refresh Token
                "user@example.com")) // User Email
            {
                // Create an appointment instance
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("attendee1@example.com"),
                    new MailAddress("attendee2@example.com")
                };

                Appointment appointment = new Appointment(
                    "Team Meeting",                     // Summary
                    new DateTime(2024, 5, 20, 10, 0, 0), // Start
                    new DateTime(2024, 5, 20, 11, 0, 0), // End
                    organizer,
                    attendees);

                appointment.Location = "Conference Room A";
                appointment.Description = "Discuss project milestones.";

                // Persist the appointment to the primary calendar
                string calendarId = "primary";
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);

                Console.WriteLine("Appointment created with UID: " + created.UniqueId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
