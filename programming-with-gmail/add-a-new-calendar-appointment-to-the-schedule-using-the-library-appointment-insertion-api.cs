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
            // Initialize Gmail client (replace placeholders with real values)
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string userEmail = "user@example.com";

            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
            {
                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    location: "Conference Room 1",
                    summary: "Project Kickoff",
                    description: "Discuss project goals and timelines.",
                    startDate: new DateTime(2024, 5, 1, 10, 0, 0),
                    endDate: new DateTime(2024, 5, 1, 11, 0, 0),
                    organizer: new MailAddress(userEmail),
                    attendees: attendees
                );

                // Calendar identifier (replace with actual calendar ID)
                string calendarId = "primary";

                // Insert the appointment into the calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine("Appointment created with UID: " + created.UniqueId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}