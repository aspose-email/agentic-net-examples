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
            // Placeholder credentials – skip execution if they are not real.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Prepare attendees.
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("person1@domain.com"));
                    attendees.Add(new MailAddress("person2@domain.com"));
                    attendees.Add(new MailAddress("person3@domain.com"));

                    // Create the appointment.
                    Appointment appointment = new Appointment(
                        location: "Room 112",
                        summary: "Team Meeting",
                        description: "Discuss project milestones",
                        startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                        endDate: new DateTime(2024, 5, 20, 11, 0, 0),
                        organizer: new MailAddress("organizer@domain.com"),
                        attendees: attendees);

                                        appointment.Summary = "Meeting Summary";
// Create the appointment on the server (invitations are sent automatically).
                    string uid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Appointment created with UID: {uid}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
