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
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            try
            {
                // Retrieve the list of calendars and pick the first one
                Calendar[] calendars = gmailClient.ListCalendars();
                if (calendars == null || calendars.Length == 0)
                {
                    Console.Error.WriteLine("No calendars available.");
                    return;
                }
                string calendarId = calendars[0].Id;

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));

                // Create a new appointment (no parameterless constructor)
                Appointment newAppointment = new Appointment(
                    "Conference Room",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);
                newAppointment.Summary = "Project Sync";
                newAppointment.Description = "Discuss project status.";

                // Create the appointment on Google Calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, newAppointment);
                Console.WriteLine($"Created appointment ID: {created.UniqueId}");

                // Retrieve the created appointment
                Appointment fetched = gmailClient.FetchAppointment(calendarId, created.UniqueId);
                Console.WriteLine($"Fetched appointment summary: {fetched.Summary}");

                // Modify the appointment
                fetched.Summary = "Updated Project Sync";
                fetched.Description = "Updated discussion points.";
                Appointment updated = gmailClient.UpdateAppointment(calendarId, fetched);
                Console.WriteLine($"Updated appointment summary: {updated.Summary}");
            }
            finally
            {
                // Ensure the client is disposed
                if (gmailClient != null)
                {
                    gmailClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
