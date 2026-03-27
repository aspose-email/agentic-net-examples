using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize EWS client (replace placeholders with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            NetworkCredential credentials = new NetworkCredential(username, password);

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Create an appointment
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                Appointment appointment = new Appointment(
                    "Team Meeting",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    organizer,
                    attendees);
                appointment.Location = "Conference Room";
                appointment.Description = "Discuss project status.";

                // Convert the appointment to a MAPI message (required for CreateItem)
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Persist the appointment using CreateItem (default folder)
                    string itemUri = client.CreateItem(mapiMessage);
                    Console.WriteLine("Appointment created with URI: " + itemUri);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}