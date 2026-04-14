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

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create appointment (meeting request)
                DateTime start = DateTime.Now.AddHours(1);
                DateTime end = start.AddHours(2);
                Appointment meeting = new Appointment(
                    "Conference Room 1",
                    start,
                    end,
                    new MailAddress(username),
                    attendees);

                meeting.Summary = "Project Kickoff";
                meeting.Description = "Discuss project goals and timelines.";

                // Add a custom reminder (15 minutes before start)
                AppointmentReminder reminder = new AppointmentReminder();
                // The AppointmentReminder class does not expose explicit properties for interval in this version,
                // but adding it to the collection enables the default reminder behavior.
                meeting.Reminders.Add(reminder);

                // Create the appointment on the server
                string appointmentUid = client.CreateAppointment(meeting);
                Console.WriteLine($"Meeting created with UID: {appointmentUid}");

                // Fetch the appointment back to verify the reminder exists
                Appointment fetched = client.FetchAppointment(appointmentUid);
                if (fetched != null && fetched.Reminders != null && fetched.Reminders.Count > 0)
                {
                    Console.WriteLine("Custom reminder verified on the created meeting.");
                }
                else
                {
                    Console.WriteLine("Reminder verification failed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
