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
            // Initialize credentials and mailbox URI
            NetworkCredential credentials = new NetworkCredential("username", "password");
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare attendees list
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Define appointment details
                DateTime start = new DateTime(2023, 12, 1, 10, 0, 0);
                DateTime end = start.AddHours(1);
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Team Meeting",
                    "Discuss project milestones",
                    start,
                    end,
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Persist the appointment to the Exchange calendar
                string uid = client.CreateAppointment(appointment);
                Console.WriteLine("Created appointment UID: " + uid);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
