using Aspose.Email.Calendar;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));

                // Create the appointment
                Appointment appointment = new Appointment(
                    location: "Conference Room 1",
                    summary: "Project Kickoff",
                    description: "Discuss project goals and timelines.",
                    startDate: new DateTime(2026, 4, 15, 10, 0, 0),
                    endDate: new DateTime(2026, 4, 15, 11, 0, 0),
                    organizer: new MailAddress("organizer@example.com"),
                    attendees: attendees
                );

                // Persist the appointment to the default calendar folder
                string calendarFolderUri = client.MailboxInfo.CalendarUri;
                string appointmentUid = client.CreateAppointment(appointment, calendarFolderUri);

                Console.WriteLine($"Appointment created with UID: {appointmentUid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
