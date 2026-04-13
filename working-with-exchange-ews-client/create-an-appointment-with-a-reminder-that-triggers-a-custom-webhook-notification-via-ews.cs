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
            // Create EWS client
            IEWSClient client;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    location: "Conference Room 1",
                    summary: "Project Sync Meeting",
                    description: "Weekly sync to discuss project status.",
                    startDate: DateTime.Now.AddHours(2),
                    endDate: DateTime.Now.AddHours(3),
                    organizer: new MailAddress("organizer@example.com"),
                    attendees: attendees);

                                appointment.Summary = "Meeting Summary";
// Add a default 15‑minute reminder (can be customized further if needed)
                appointment.Reminders.Add(AppointmentReminder.Default15MinReminder);

                // Create the appointment on the server
                string uid;
                try
                {
                    uid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Appointment created with UID: {uid}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
