using Aspose.Email.Calendar;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials/host
            if (exchangeUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder Exchange server details detected. Skipping send operation.");
                return;
            }

            // Create appointment
            MailAddress organizer = new MailAddress("organizer@domain.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@domain.com"));
            attendees.Add(new MailAddress("attendee2@domain.com"));
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2024, 5, 1, 10, 0, 0),
                new DateTime(2024, 5, 1, 11, 0, 0),
                organizer,
                attendees);
            appointment.Location = "Conference Room";
            appointment.Description = "Discuss project status.";

            // Convert appointment to MailMessage
            using (MailMessage message = appointment.ToMailMessage())
            {
                message.From = organizer;
                message.Subject = "Meeting Request: Team Meeting";

                // Send using ExchangeClient
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    client.Send(message);
                }
            }

            Console.WriteLine("Meeting request sent successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
