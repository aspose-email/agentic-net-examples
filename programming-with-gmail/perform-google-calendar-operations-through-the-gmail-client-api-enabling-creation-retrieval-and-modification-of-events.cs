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
            // Initialize Gmail client with dummy OAuth credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // Create a new calendar and obtain its identifier
            Calendar calendar = new Calendar("Sample Calendar");
            string calendarId = gmailClient.CreateCalendar(calendar);

            // Prepare attendees for the appointment
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create an appointment instance using the correct constructor
            Appointment appointment = new Appointment(
                "Conference Room",                     // location
                "Team Meeting",                        // summary
                "Discuss project updates",             // description
                new DateTime(2024, 5, 1, 10, 0, 0),    // start date/time
                new DateTime(2024, 5, 1, 11, 0, 0),    // end date/time
                new MailAddress("organizer@example.com"), // organizer
                attendees);                            // attendees

            // Create the appointment in the newly created calendar
            Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
            Console.WriteLine("Created appointment ID: " + created.UniqueId);

            // Retrieve and list all appointments in the calendar
            Appointment[] appointments = gmailClient.ListAppointments(calendarId);
            foreach (Appointment appt in appointments)
            {
                Console.WriteLine($"Appointment: {appt.Summary}, ID: {appt.UniqueId}");
            }

            // Update the appointment (e.g., change the summary)
            created.Summary = "Updated Team Meeting";
            Appointment updated = gmailClient.UpdateAppointment(calendarId, created);
            Console.WriteLine("Updated appointment summary: " + updated.Summary);

            // Clean up: delete the appointment and the calendar
            gmailClient.DeleteAppointment(calendarId, updated.UniqueId);
            gmailClient.DeleteCalendar(calendarId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
