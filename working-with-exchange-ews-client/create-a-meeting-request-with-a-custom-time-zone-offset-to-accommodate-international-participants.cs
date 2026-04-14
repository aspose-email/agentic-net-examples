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
            // Placeholder credentials – skip network call if they are not replaced
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            bool isPlaceholder = string.IsNullOrWhiteSpace(mailboxUri) ||
                                 mailboxUri.Contains("example.com") ||
                                 username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                 password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
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

            using (client as IDisposable)
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@domain.com"));
                attendees.Add(new MailAddress("attendee2@domain.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    "Project Kickoff",
                    new DateTime(2024, 12, 15, 9, 0, 0),
                    new DateTime(2024, 12, 15, 10, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);

                appointment.Location = "Conference Room A";
                appointment.Description = "Discuss project goals and timelines.";

                // Convert to MailMessage to set custom UTC offset (+05:30)
                MailMessage meetingMessage = appointment.ToMailMessage();
                meetingMessage.TimeZoneOffset = new TimeSpan(5, 30, 0); // UTC+05:30

                // Send meeting request via EWS
                try
                {
                    client.Send(meetingMessage);
                    Console.WriteLine("Meeting request sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send meeting request: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
