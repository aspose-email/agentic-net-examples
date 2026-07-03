using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "YOUR_EWS_HOST";
            string username = "YOUR_USERNAME";
            string password = "YOUR_PASSWORD";
            string domain = "YOUR_DOMAIN";

            // Skip execution if placeholders are detected.
            if (host.StartsWith("YOUR_") || username.StartsWith("YOUR_") || password.StartsWith("YOUR_") || domain.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS client operation.");
                return;
            }

            // Create Exchange Web Services client.
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password, domain))
            {
                // Prepare organizer and attendees.
                MailAddress organizer = new MailAddress(username);
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("attendee1@example.com"),
                    new MailAddress("attendee2@example.com")
                };

                // Define start and end times.
                DateTime start = DateTime.Now.AddHours(1);
                DateTime end = start.AddHours(2);

                // Create appointment.
                Appointment appointment = new Appointment(
                    "Conference Room",          // location
                    "Team Meeting",            // summary (title)
                    "Discuss project status",  // description
                    start,
                    end,
                    organizer,
                    attendees);

                appointment.Summary = "Meeting Summary";

                // Create the appointment in the default calendar.
                string appointmentId = client.CreateAppointment(appointment);
                Console.WriteLine($"Appointment created with ID: {appointmentId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
