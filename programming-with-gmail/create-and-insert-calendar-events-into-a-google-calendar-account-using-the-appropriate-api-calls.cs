using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client (replace placeholders with real credentials)
            IGmailClient gmailClient = GmailClient.GetInstance(
                clientId: "YOUR_CLIENT_ID",
                clientSecret: "YOUR_CLIENT_SECRET",
                refreshToken: "YOUR_REFRESH_TOKEN",
                defaultEmail: "your.email@example.com");

            // Use the client within a using block to ensure disposal
            using (gmailClient)
            {
                try
                {
                    // Calendar identifier (replace with actual calendar ID)
                    string calendarId = "primary";

                    // Prepare attendees
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("person1@example.com"));
                    attendees.Add(new MailAddress("person2@example.com"));
                    attendees.Add(new MailAddress("person3@example.com"));

                    // Create an appointment
                    Appointment appointment = new Appointment(
                        location: "Conference Room 1",
                        startDate: new DateTime(2024, 12, 15, 10, 0, 0),
                        endDate: new DateTime(2024, 12, 15, 11, 0, 0),
                        organizer: new MailAddress("organizer@example.com"),
                        attendees: attendees);

                    appointment.Summary = "Project Kickoff Meeting";
                    appointment.Description = "Discuss project goals, timeline, and responsibilities.";

                    // Insert the appointment into the specified Google Calendar
                    Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                    Console.WriteLine("Appointment created with ID: " + created.UniqueId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Gmail operation failed: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}