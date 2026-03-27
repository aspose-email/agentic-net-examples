using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Google;

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

            using (gmailClient)
            {
                // ----- Create a new calendar -----
                Calendar newCalendar = new Calendar("Sample Calendar");
                string calendarId = gmailClient.CreateCalendar(newCalendar);
                // Store the identifier in the calendar object for later use
                newCalendar.Id = calendarId;

                Console.WriteLine($"Created calendar with Id: {calendarId}");

                // ----- List existing calendars -----
                Calendar[] calendars = gmailClient.ListCalendars();
                Console.WriteLine("Existing calendars:");
                foreach (Calendar cal in calendars)
                {
                    Console.WriteLine($"- {cal.Summary} (Id: {cal.Id})");
                }

                // ----- Prepare attendees for the appointment -----
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // ----- Create a new appointment -----
                Appointment appointment = new Appointment(
                    "Team Meeting",                     // summary
                    "Discuss project status",           // description
                    "Conference Room A",                // location
                    DateTime.Now.AddHours(1),           // start time
                    DateTime.Now.AddHours(2),           // end time
                    new MailAddress("organizer@example.com"),
                    attendees);

                Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine($"Created appointment with Id: {createdAppointment.UniqueId}");

                // ----- List appointments in the calendar -----
                Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                Console.WriteLine("Appointments in the calendar:");
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"- {appt.Summary} (Id: {appt.UniqueId})");
                }

                // ----- Fetch the created appointment -----
                Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                Console.WriteLine($"Fetched appointment: {fetchedAppointment.Summary} at {fetchedAppointment.StartDate}");

                // ----- Update the appointment -----
                fetchedAppointment.Summary = "Updated Team Meeting";
                fetchedAppointment.Location = "Conference Room B";
                Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                Console.WriteLine($"Updated appointment summary: {updatedAppointment.Summary}, location: {updatedAppointment.Location}");

                // ----- Delete the appointment -----
                gmailClient.DeleteAppointment(calendarId, updatedAppointment.UniqueId);
                Console.WriteLine("Deleted the appointment.");

                // ----- Delete the calendar -----
                gmailClient.DeleteCalendar(calendarId);
                Console.WriteLine("Deleted the calendar.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
