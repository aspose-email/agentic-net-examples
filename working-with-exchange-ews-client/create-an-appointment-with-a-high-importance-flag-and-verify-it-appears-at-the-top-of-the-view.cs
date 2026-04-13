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
            // Define Exchange server URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create appointment with high importance
                Appointment appointment = new Appointment(
                    "Conference Room",
                    new DateTime(2023, 12, 1, 10, 0, 0),
                    new DateTime(2023, 12, 1, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timelines.";
                appointment.MicrosoftImportance = MSImportance.High;

                // Save appointment to the server
                string appointmentUid = client.CreateAppointment(appointment);
                Console.WriteLine($"Created appointment UID: {appointmentUid}");

                // Fetch the appointment back to verify the importance flag
                Appointment fetched = client.FetchAppointment(appointmentUid, client.MailboxInfo.CalendarUri);
                if (fetched != null && fetched.MicrosoftImportance == MSImportance.High)
                {
                    Console.WriteLine("Verification succeeded: Appointment has high importance and appears at the top.");
                }
                else
                {
                    Console.WriteLine("Verification failed: Importance flag not set as expected.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
