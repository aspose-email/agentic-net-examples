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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Required attendees
                MailAddressCollection requiredAttendees = new MailAddressCollection();
                requiredAttendees.Add(new MailAddress("required1@example.com"));
                requiredAttendees.Add(new MailAddress("required2@example.com"));

                // Optional attendees
                MailAddressCollection optionalAttendees = new MailAddressCollection();
                optionalAttendees.Add(new MailAddress("optional1@example.com"));
                optionalAttendees.Add(new MailAddress("optional2@example.com"));

                // Create appointment with required attendees
                Appointment appointment = new Appointment(
                    "Conference Room",
                    new DateTime(2023, 12, 15, 10, 0, 0),
                    new DateTime(2023, 12, 15, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    requiredAttendees);

                // Add optional attendees via the read‑only collection
                foreach (MailAddress addr in optionalAttendees)
                {
                    appointment.OptionalAttendees.Add(addr);
                }

                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timeline.";
                appointment.Location = "Conference Room";

                // Enable tracking request
                appointment.MethodType = AppointmentMethodType.Request;

                // Convert appointment to a meeting request (alternate view)
                AlternateView meetingRequest = appointment.RequestApointment();

                // Create mail message and attach the meeting request
                MailMessage message = new MailMessage();
                message.From = new MailAddress("organizer@example.com");
                foreach (MailAddress addr in requiredAttendees)
                {
                    message.To.Add(addr);
                }
                foreach (MailAddress addr in optionalAttendees)
                {
                    message.To.Add(addr);
                }
                message.Subject = appointment.Summary;
                message.AlternateViews.Add(meetingRequest);

                // Send the meeting request
                client.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
