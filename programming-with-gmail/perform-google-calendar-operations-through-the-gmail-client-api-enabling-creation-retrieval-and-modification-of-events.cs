using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GoogleCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client (replace placeholders with real credentials)
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string userEmail = "user@example.com";

                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail);

                // List existing calendars
                Aspose.Email.Clients.Google.Calendar[] existingCalendars = gmailClient.ListCalendars();
                Console.WriteLine("Existing calendars:");
                foreach (Aspose.Email.Clients.Google.Calendar cal in existingCalendars)
                {
                    Console.WriteLine($"- Id: {cal.Id}, Summary: {cal.Summary}");
                }

                // Create a new calendar
                Aspose.Email.Clients.Google.Calendar newCalendar = new Aspose.Email.Clients.Google.Calendar("Sample Calendar");
                string calendarId = gmailClient.CreateCalendar(newCalendar);
                Console.WriteLine($"Created calendar with Id: {calendarId}");

                // Fetch the created calendar
                ExtendedCalendar fetchedCalendar = gmailClient.FetchCalendar(calendarId);
                Console.WriteLine($"Fetched calendar: Id={fetchedCalendar.Id}, Summary={fetchedCalendar.Summary}");

                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    "Team Meeting",
                    DateTime.Now.AddDays(1).AddHours(9),
                    DateTime.Now.AddDays(1).AddHours(10),
                    new MailAddress(userEmail),
                    attendees);
                appointment.Summary = "Weekly Team Sync";
                appointment.Description = "Discuss project updates and next steps.";

                // Add the appointment to the calendar
                Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine($"Created appointment Id: {createdAppointment.UniqueId}");

                // List appointments in the calendar
                Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                Console.WriteLine("Appointments in the calendar:");
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"- Id: {appt.UniqueId}, Summary: {appt.Summary}, Start: {appt.StartDate}");
                }

                // Fetch the specific appointment
                Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                Console.WriteLine($"Fetched appointment: Id={fetchedAppointment.UniqueId}, Summary={fetchedAppointment.Summary}");

                // Update the appointment (change the summary)
                fetchedAppointment.Summary = "Updated Team Sync";
                Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                Console.WriteLine($"Updated appointment: Id={updatedAppointment.UniqueId}, New Summary={updatedAppointment.Summary}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}