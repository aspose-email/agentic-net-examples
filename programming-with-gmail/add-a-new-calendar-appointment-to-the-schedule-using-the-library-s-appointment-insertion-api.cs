using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance("clientId", "clientSecret", "refreshToken", "user@example.com"))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    "Conference Room 101",
                    new DateTime(2023, 12, 1, 10, 0, 0),
                    new DateTime(2023, 12, 1, 11, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                appointment.Summary = "Team Sync";
                appointment.Description = "Weekly team synchronization meeting.";

                // Calendar identifier (use "primary" as a placeholder)
                string calendarId = "primary";

                // Add the appointment to the calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine("Appointment created. Unique ID: " + created.UniqueId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
