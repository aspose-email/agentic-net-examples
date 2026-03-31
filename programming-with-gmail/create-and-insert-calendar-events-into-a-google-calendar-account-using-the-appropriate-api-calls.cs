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
            // Placeholder credentials – replace with real values when running in a real environment.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid external calls during CI.
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Google Calendar operation.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            using (gmailClient as IDisposable)
            {
                // Define the calendar identifier where the event will be created.
                string calendarId = "primary"; // Use "primary" for the default calendar or replace with a specific ID.

                // Prepare attendees.
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create an appointment (event).
                Appointment appointment = new Appointment(
                    "Team Sync Meeting",                     // Summary
                    "Discuss project updates and plans.",    // Description
                    "Conference Room A",                     // Location
                    new DateTime(2024, 5, 20, 10, 0, 0),     // Start date and time
                    new DateTime(2024, 5, 20, 11, 0, 0),     // End date and time
                    new MailAddress(defaultEmail),           // Organizer
                    attendees);                             // Attendees

                // Insert the appointment into the specified Google Calendar.
                try
                {
                    Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                    Console.WriteLine("Appointment created with ID: " + createdAppointment.UniqueId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create appointment: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
