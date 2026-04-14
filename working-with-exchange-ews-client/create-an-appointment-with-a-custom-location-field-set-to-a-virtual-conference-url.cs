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
            // Mailbox connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
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

                // Create the appointment
                Appointment appointment = new Appointment(
                    "Online",                                   // initial location (will be overridden)
                    new DateTime(2024, 5, 20, 10, 0, 0),        // start time
                    new DateTime(2024, 5, 20, 11, 0, 0),        // end time
                    new MailAddress("organizer@example.com"),   // organizer
                    attendees);                                 // attendees

                // Set custom location to a virtual conference URL
                appointment.Location = "https://virtualconf.example.com/meeting";

                // Additional details
                appointment.Summary = "Team Sync Meeting";
                appointment.Description = "Discuss project updates and next steps.";

                // Create the appointment on the server
                string appointmentUid;
                try
                {
                    appointmentUid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Appointment created with UID: {appointmentUid}");
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
