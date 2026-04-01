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
            // Placeholder credentials – replace with real values for actual execution
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard to avoid making real network calls with placeholder credentials
            if (clientId == "clientId" || clientSecret == "clientSecret")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail API calls.");
                return;
            }

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Prepare appointment details
                MailAddress organizer = new MailAddress(defaultEmail);
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                DateTime start = DateTime.UtcNow.AddHours(1);
                DateTime end = start.AddHours(2);
                Appointment newAppointment = new Appointment("Meeting Subject", start, end, organizer, attendees);
                newAppointment.Description = "Discuss project updates.";
                newAppointment.Location = "Conference Room";

                // Calendar identifier (use "primary" for the main calendar)
                string calendarId = "primary";

                // Create the appointment in Gmail calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, newAppointment);
                Console.WriteLine($"Created appointment with ID: {created.UniqueId}");

                // List all appointments in the calendar
                Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                Console.WriteLine($"Total appointments: {appointments.Length}");
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"- {appt.Summary} ({appt.StartDate} - {appt.EndDate}) ID: {appt.UniqueId}");
                }

                // Retrieve the newly created appointment by its unique identifier
                Appointment fetched = gmailClient.FetchAppointment(calendarId, created.UniqueId);
                Console.WriteLine($"Fetched appointment: {fetched.Summary}");

                // Modify the appointment
                fetched.Location = "Updated Conference Room";
                fetched.Description = "Updated description.";
                Appointment updated = gmailClient.UpdateAppointment(calendarId, fetched);
                Console.WriteLine($"Updated appointment location: {updated.Location}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
