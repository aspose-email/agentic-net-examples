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
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange: {ex.Message}");
                return;
            }

            using (client)
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));
                attendees.Add(new MailAddress("carol@example.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    "Conference Room 1",
                    new DateTime(2026, 5, 20, 10, 0, 0),
                    new DateTime(2026, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Set meeting details
                appointment.Summary = "Project Kickoff Meeting";
                appointment.Description = "Agenda:\n1. Introductions\n2. Project Overview\n3. Timeline Discussion\n4. Q&A";

                // Add a reminder 15 minutes before start
                appointment.Reminders.Add(new AppointmentReminder());

                // Create the appointment on the server (invitations are sent automatically)
                try
                {
                    string appointmentUid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Meeting request sent. Appointment UID: {appointmentUid}");
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
