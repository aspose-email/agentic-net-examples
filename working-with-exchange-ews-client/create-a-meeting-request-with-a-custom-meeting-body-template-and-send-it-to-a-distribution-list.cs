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
            // Connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
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
                // Distribution list as attendee
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("team-distribution@example.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    "Conference Room A",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress(username),
                    attendees);

                appointment.Summary = "Quarterly Planning Session";

                // Custom HTML body template
                appointment.HtmlDescription = "<html><body><h2>Agenda</h2><ul><li>Review Q1 results</li><li>Set goals for Q2</li></ul></body></html>";

                // Create appointment on server (sends invitations)
                string appointmentUid = client.CreateAppointment(appointment);
                Console.WriteLine("Meeting created with UID: " + appointmentUid);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
