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
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create the EWS client using the verified factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare appointment details.
                MailAddress organizer = new MailAddress(username);
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                Appointment appointment = new Appointment(
                    "Conference Room",                     // location
                    "Team Meeting",                        // summary
                    "Discuss project status",              // description
                    DateTime.Now.AddHours(1),              // start time
                    DateTime.Now.AddHours(2),              // end time
                    organizer,
                    attendees);

                // ---- Create appointment ----
                string appointmentId = client.CreateAppointment(appointment);
                Console.WriteLine($"Created appointment UID: {appointmentId}");

                // ---- Retrieve appointment ----
                Appointment fetched = client.FetchAppointment(appointmentId);
                Console.WriteLine($"Fetched appointment summary: {fetched.Summary}");

                // ---- Update appointment ----
                fetched.Summary = "Updated Team Meeting";
                client.UpdateAppointment(fetched);
                Console.WriteLine("Appointment updated.");

                // ---- Delete (cancel) appointment ----
                client.CancelAppointment(appointmentId);
                Console.WriteLine("Appointment cancelled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
