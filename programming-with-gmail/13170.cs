using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholder credentials are detected
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail calendar operations.");
                return;
            }

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Define calendar identifier (primary calendar)
                string calendarId = "primary";

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create an appointment
                DateTime start = new DateTime(2024, 5, 1, 10, 0, 0);
                DateTime end = new DateTime(2024, 5, 1, 11, 0, 0);
                Appointment appointment = new Appointment(
                    "Team Meeting",                     // Summary
                    "Discuss project status",           // Description
                    "Conference Room A",                // Location
                    start,
                    end,
                    new MailAddress(defaultEmail),      // Organizer
                    attendees);

                // Create the appointment in the calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine($"Created appointment ID: {created.UniqueId}");

                // Modify the appointment (e.g., change location)
                created.Location = "Conference Room B";
                Appointment updated = gmailClient.UpdateAppointment(calendarId, created);
                Console.WriteLine($"Updated appointment location: {updated.Location}");

                // Delete the appointment
                string appointmentId = created.UniqueId;
                gmailClient.DeleteAppointment(calendarId, appointmentId);
                Console.WriteLine("Deleted appointment.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
