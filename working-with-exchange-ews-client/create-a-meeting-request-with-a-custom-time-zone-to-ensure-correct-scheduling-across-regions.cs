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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(username) ||
                username.Contains("example") ||
                password.Contains("password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            ICredentials credentials = new NetworkCredential(username, password);

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    "Team Meeting",
                    new DateTime(2024, 12, 15, 10, 0, 0),
                    new DateTime(2024, 12, 15, 11, 0, 0),
                    new MailAddress(username),
                    attendees);

                appointment.Summary = "Quarterly Review";
                appointment.Description = "Discuss quarterly performance and goals.";
                // Set custom time zone
                appointment.SetTimeZone("Pacific Standard Time");

                // Send the meeting request
                string uid = client.CreateAppointment(appointment);
                Console.WriteLine($"Meeting created with UID: {uid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
