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
            IGmailClient gmailClient = null;
            try
            {
                // Initialize Gmail client with dummy credentials
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

            using (gmailClient)
            {
                string calendarId = "primary";

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create a new appointment
                Appointment newAppointment = new Appointment(
                    "Conference Room",
                    DateTime.Now.AddHours(1),
                    DateTime.Now.AddHours(2),
                    new MailAddress("organizer@example.com"),
                    attendees);
                newAppointment.Summary = "Team Sync";
                newAppointment.Description = "Weekly team sync meeting.";

                Appointment createdAppointment;
                try
                {
                    createdAppointment = gmailClient.CreateAppointment(calendarId, newAppointment);
                    Console.WriteLine($"Created appointment ID: {createdAppointment.UniqueId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
                    return;
                }

                // List all appointments in the calendar
                Appointment[] appointments;
                try
                {
                    appointments = gmailClient.ListAppointments(calendarId);
                    Console.WriteLine($"Total appointments in calendar: {appointments.Length}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                // Fetch the created appointment by its ID
                Appointment fetchedAppointment;
                try
                {
                    fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                    Console.WriteLine($"Fetched appointment summary: {fetchedAppointment.Summary}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch appointment: {ex.Message}");
                    return;
                }

                // Update the appointment's summary
                fetchedAppointment.Summary = "Updated Team Sync";
                try
                {
                    Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                    Console.WriteLine($"Updated appointment summary: {updatedAppointment.Summary}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update appointment: {ex.Message}");
                }

                // Delete the appointment
                try
                {
                    gmailClient.DeleteAppointment(calendarId, createdAppointment.UniqueId);
                    Console.WriteLine("Appointment deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete appointment: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
