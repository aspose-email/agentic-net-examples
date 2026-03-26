using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace GoogleCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client (replace placeholders with real credentials)
                using (IGmailClient gmailClient = GmailClient.GetInstance(
                    "YOUR_CLIENT_ID",
                    "YOUR_CLIENT_SECRET",
                    "YOUR_REFRESH_TOKEN",
                    "user@example.com"))
                {
                    // -------------------------------------------------
                    // 1. Create a new calendar and obtain its identifier
                    // -------------------------------------------------
                    Calendar newCalendar = new Calendar("Sample Calendar");
                    string calendarId = gmailClient.CreateCalendar(newCalendar);
                    Console.WriteLine("Created calendar with Id: " + calendarId);

                    // -------------------------------------------------
                    // 2. Create an appointment
                    // -------------------------------------------------
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    Appointment appointment = new Appointment(
                        "Conference Room",
                        new DateTime(2024, 5, 20, 10, 0, 0),
                        new DateTime(2024, 5, 20, 11, 0, 0),
                        new MailAddress("organizer@example.com"),
                        attendees);
                    appointment.Summary = "Project Kickoff";
                    appointment.Description = "Discuss project goals and timeline.";

                    // Create the appointment in the previously created calendar
                    Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                    Console.WriteLine("Created appointment with Id: " + createdAppointment.UniqueId);

                    // -------------------------------------------------
                    // 3. Retrieve the appointment by its Id
                    // -------------------------------------------------
                    Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                    Console.WriteLine("Fetched appointment summary: " + fetchedAppointment.Summary);

                    // -------------------------------------------------
                    // 4. Update the appointment (e.g., change the summary)
                    // -------------------------------------------------
                    fetchedAppointment.Summary = "Updated Project Kickoff";
                    Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                    Console.WriteLine("Updated appointment summary: " + updatedAppointment.Summary);

                    // -------------------------------------------------
                    // 5. Delete the appointment
                    // -------------------------------------------------
                    gmailClient.DeleteAppointment(calendarId, updatedAppointment.UniqueId);
                    Console.WriteLine("Deleted appointment with Id: " + updatedAppointment.UniqueId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}