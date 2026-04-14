using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings
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
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@domain.com"));
            attendees.Add(new MailAddress("attendee2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Location = "Conference Room";
            appointment.Description = "Discuss project status.";
            appointment.Summary = "Team Meeting";
            // Request responses from attendees
            appointment.MethodType = AppointmentMethodType.Request;
            // Note: Automatic decline on conflict is handled by server settings; no client‑side flag is required.

            // Send the meeting invitation
            try
            {
                string appointmentUid = client.CreateAppointment(appointment);
                Console.WriteLine($"Meeting invitation sent. UID: {appointmentUid}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send meeting invitation: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
