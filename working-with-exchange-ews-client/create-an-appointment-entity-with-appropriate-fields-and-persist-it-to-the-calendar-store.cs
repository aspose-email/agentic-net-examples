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
            IGmailClient gmailClient;
            try
            {
                gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Build appointment details
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Conference Room A",
                new DateTime(2024, 5, 20, 10, 0, 0),
                new DateTime(2024, 5, 20, 11, 0, 0),
                organizer,
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timelines.";
            appointment.Location = "Conference Room A";

            // Persist the appointment to a calendar (using a placeholder calendar ID)
            string calendarId = "primary";
            try
            {
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine($"Appointment created with UID: {created.UniqueId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
            }

            // Dispose client if it implements IDisposable
            if (gmailClient is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
