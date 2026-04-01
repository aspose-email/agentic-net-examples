using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – in real scenario replace with actual values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are used.
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create the appointment to be persisted.
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2024, 10, 15, 10, 0, 0),
                new DateTime(2024, 10, 15, 11, 0, 0),
                organizer,
                attendees);
            appointment.Summary = "Quarterly Planning";
            appointment.Description = "Discuss quarterly goals and milestones.";

            // Create and use the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Persist the appointment on the server.
                    string appointmentUid = client.CreateAppointment(appointment);
                    Console.WriteLine("Appointment created with UID: " + appointmentUid);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error creating appointment: " + ex.Message);
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
