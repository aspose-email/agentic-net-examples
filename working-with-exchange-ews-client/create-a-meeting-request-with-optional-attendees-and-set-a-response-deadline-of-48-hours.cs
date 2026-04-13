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
            // Placeholder values – replace with real credentials or skip when placeholders are used
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials
            if (mailboxUri.Contains("example") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Build the meeting request
            DateTime start = DateTime.Now.AddDays(1);
            DateTime end = start.AddHours(2);
            MailAddress organizer = new MailAddress("organizer@domain.com");
            MailAddressCollection requiredAttendees = new MailAddressCollection();
            requiredAttendees.Add(new MailAddress("required1@domain.com"));
            requiredAttendees.Add(new MailAddress("required2@domain.com"));

            Appointment meeting = new Appointment("Conference Room", start, end, organizer, requiredAttendees);
            meeting.Summary = "Project Kickoff";
            meeting.Description = "Discuss project goals and timeline.";

            // Optional attendees
            meeting.OptionalAttendees.Add(new MailAddress("optional1@domain.com"));
            meeting.OptionalAttendees.Add(new MailAddress("optional2@domain.com"));

            // Request responses – Aspose.Email includes a default response deadline (48 hours) when responses are requested.
            // The API does not expose a direct property for the deadline, so we rely on the default behavior.

            // Create the meeting on the server
            try
            {
                string appointmentId = client.CreateAppointment(meeting);
                Console.WriteLine($"Meeting created with ID: {appointmentId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
