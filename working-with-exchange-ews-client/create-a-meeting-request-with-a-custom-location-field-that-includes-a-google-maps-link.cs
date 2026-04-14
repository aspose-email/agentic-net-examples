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
            // Exchange server connection details
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
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create appointment with a custom location that includes a Google Maps link
                Appointment appointment = new Appointment(
                    "Team Sync",
                    new DateTime(2024, 12, 1, 10, 0, 0),
                    new DateTime(2024, 12, 1, 11, 0, 0),
                    new MailAddress(username),
                    attendees);

                appointment.Location = "https://maps.google.com/?q=1600+Amphitheatre+Parkway,+Mountain+View,+CA";
                appointment.Summary = "Project Kickoff Meeting";
                appointment.Description = "Discuss project goals and timelines.";

                // Create the appointment on the server (invites are sent automatically)
                string appointmentId = client.CreateAppointment(appointment);
                Console.WriteLine("Appointment created with ID: " + appointmentId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
