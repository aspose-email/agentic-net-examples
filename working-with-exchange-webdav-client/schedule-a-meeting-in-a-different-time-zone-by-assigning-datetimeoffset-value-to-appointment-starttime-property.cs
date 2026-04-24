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
            // Placeholder service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Define start and end times with a specific offset (e.g., UTC+2)
                DateTimeOffset startOffset = new DateTimeOffset(2023, 12, 25, 10, 0, 0, TimeSpan.FromHours(2));
                DateTimeOffset endOffset = startOffset.AddHours(2);

                // Create the appointment
                Appointment appointment = new Appointment(
                    location: "Conference Room",
                    summary: "Project Kickoff",
                    description: "Discuss project goals and timeline.",
                    startDate: startOffset.DateTime,
                    endDate: endOffset.DateTime,
                    organizer: new MailAddress(username),
                    attendees: attendees);

                                appointment.Summary = "Meeting Summary";
// Assign the corresponding time zone
                appointment.StartTimeZone = "Etc/GMT-2"; // GMT+2
                appointment.EndTimeZone = "Etc/GMT-2";

                // Create the appointment on the Exchange server
                string uid = client.CreateAppointment(appointment);
                Console.WriteLine($"Appointment created with UID: {uid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
