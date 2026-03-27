using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace AsposeEmailGmailCalendarSample
{
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

                using (gmailClient)
                {
                    // Create a new calendar
                    Calendar newCalendar = new Calendar("Sample Calendar");
                    string calendarId = gmailClient.CreateCalendar(newCalendar);

                    // Prepare attendees collection
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    // Create an appointment
                    Appointment appointment = new Appointment(
                        "Conference Room",
                        new DateTime(2024, 5, 20, 10, 0, 0),
                        new DateTime(2024, 5, 20, 11, 0, 0),
                        new MailAddress("organizer@example.com"),
                        attendees);
                    appointment.Summary = "Project Kickoff";
                    appointment.Description = "Discuss project goals and milestones.";

                    // Create the appointment in the calendar
                    Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                    Console.WriteLine("Created appointment ID: " + createdAppointment.UniqueId);

                    // Update the appointment (e.g., change the summary)
                    createdAppointment.Summary = "Updated Project Kickoff";
                    Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, createdAppointment);
                    Console.WriteLine("Updated appointment summary: " + updatedAppointment.Summary);

                    // Retrieve the appointment by its ID
                    Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, updatedAppointment.UniqueId);
                    Console.WriteLine("Fetched appointment summary: " + fetchedAppointment.Summary);
                    Console.WriteLine("Start: " + fetchedAppointment.StartDate);
                    Console.WriteLine("End: " + fetchedAppointment.EndDate);

                    // Delete the appointment
                    gmailClient.DeleteAppointment(calendarId, fetchedAppointment.UniqueId);
                    Console.WriteLine("Appointment deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
