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
            // Exchange server connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
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

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Define meeting times
                DateTime startTime = new DateTime(2024, 12, 1, 10, 0, 0);
                DateTime endTime = new DateTime(2024, 12, 1, 11, 0, 0);

                // Create the appointment (meeting request)
                Appointment appointment = new Appointment(
                    "Virtual Conference",          // Subject
                    startTime,
                    endTime,
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Set custom location to a virtual conference URL
                appointment.Location = "https://virtualconference.example.com/meeting123";

                // Additional details
                appointment.Summary = "Project Kickoff Meeting";
                appointment.Description = "Discuss project goals and deliverables.";

                // Create the appointment on the server (sends invitations)
                string appointmentUid = null;
                try
                {
                    appointmentUid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Meeting created successfully. UID: {appointmentUid}");
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
