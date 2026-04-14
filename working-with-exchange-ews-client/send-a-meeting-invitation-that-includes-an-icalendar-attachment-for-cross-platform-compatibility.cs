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
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("example.com") ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create the appointment.
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            Appointment appointment = new Appointment(
                "Team Meeting",                                   // Subject
                new DateTime(2024, 5, 20, 10, 0, 0),              // Start
                new DateTime(2024, 5, 20, 11, 0, 0),              // End
                new MailAddress("organizer@domain.com"),          // Organizer
                attendees);                                       // Required attendees

            appointment.Location = "Conference Room 1";
            appointment.Description = "Discuss project milestones.";

            // Convert the appointment to a MIME message (iCalendar attachment is embedded).
            MailMessage message = appointment.ToMailMessage();

            // Set the sender and recipients for the email.
            message.From = new MailAddress("organizer@domain.com");
            message.To.AddRange(attendees);

            // Send the meeting invitation via EWS.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Meeting invitation sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
