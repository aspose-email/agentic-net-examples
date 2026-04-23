using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client inside a using block for proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Enable server to return the request identifier.
                    client.ReturnClientRequestId = true;

                    // Prepare attendees.
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@domain.com"));
                    attendees.Add(new MailAddress("attendee2@domain.com"));

                    // Create the meeting request (appointment).
                    Appointment meeting = new Appointment(
                        "Conference Room",
                        new DateTime(2024, 12, 15, 10, 0, 0),
                        new DateTime(2024, 12, 15, 11, 0, 0),
                        new MailAddress("organizer@domain.com"),
                        attendees);

                    meeting.Summary = "Project Kickoff";
                    meeting.Description = "Discuss project goals and timelines.";

                    // Send the meeting request to the server.
                    client.CreateAppointment(meeting);

                    // Log the unique identifier assigned by the server.
                    Console.WriteLine("Meeting request sent. Server UniqueId: " + meeting.UniqueId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during meeting request: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
