using System;
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
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@example.com"));
                attendees.Add(new MailAddress("person2@example.com"));

                // Create an appointment (subject, start, end, organizer, attendees)
                Appointment appointment = new Appointment(
                    "Team Sync",
                    new DateTime(2024, 5, 1, 10, 0, 0),
                    new DateTime(2024, 5, 1, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                appointment.Description = "Weekly team synchronization meeting.";
                appointment.Location = "Conference Room A";

                // Create the appointment in the primary calendar
                Appointment created = gmailClient.CreateAppointment("primary", appointment);
                Console.WriteLine($"Created appointment with ID: {created.UniqueId}");

                // List all appointments in the primary calendar
                Appointment[] appointments = gmailClient.ListAppointments("primary");
                Console.WriteLine("Current appointments in primary calendar:");
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"- {appt.Summary} (ID: {appt.UniqueId})");
                }

                // Update the created appointment's summary
                created.Summary = "Team Sync - Updated";
                Appointment updated = gmailClient.UpdateAppointment("primary", created);
                Console.WriteLine($"Updated appointment summary to: {updated.Summary}");

                // Delete the appointment
                gmailClient.DeleteAppointment("primary", updated.UniqueId);
                Console.WriteLine("Deleted the appointment.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
