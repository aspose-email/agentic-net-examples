using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Service URL and credentials for EWS
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    "Team Meeting",
                    new DateTime(2024, 12, 1, 10, 0, 0),
                    new DateTime(2024, 12, 1, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);
                appointment.Location = "Conference Room 1";
                appointment.Description = "Discuss project milestones.";

                // Convert the appointment to a MAPI message
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Persist the appointment using CreateItem
                    client.CreateItem(mapiMessage);
                    Console.WriteLine("Appointment created successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
