using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // EWS client configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential(username, password);

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build the appointment (meeting invitation)
                MailAddress organizer = new MailAddress(username);
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("attendee1@example.com"),
                    new MailAddress("attendee2@example.com")
                };

                Appointment appointment = new Appointment(
                    "Team Sync",
                    new DateTime(2026, 5, 15, 10, 0, 0),
                    new DateTime(2026, 5, 15, 11, 0, 0),
                    organizer,
                    attendees)
                {
                    Location = "Conference Room 1",
                    Description = "Weekly team sync meeting."
                };

                // Convert the appointment to a MailMessage (iCalendar attached)
                MailMessage meetingMessage = appointment.ToMailMessage();

                // Send the meeting invitation
                client.Send(meetingMessage);
                Console.WriteLine("Meeting invitation sent.");

                // Prepare a query to find acceptance responses (subject contains "Accepted")
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.Subject.Contains("Accepted");
                MailQuery acceptanceQuery = queryBuilder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                string inboxUri = client.MailboxInfo.InboxUri;
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, acceptanceQuery);

                Console.WriteLine("Acceptance responses:");
                foreach (var msgInfo in messages)
                {
                    // Fetch the full message to read details
                    MailMessage response = client.FetchMessage(msgInfo.UniqueUri);
                    Console.WriteLine($"From: {response.From}");
                    Console.WriteLine($"Subject: {response.Subject}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
