using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange server URI and user credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential(username, password);

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Prepare attendees
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    // Create appointment
                    Appointment appointment = new Appointment(
                        "Conference Room",
                        new DateTime(2023, 12, 15, 10, 0, 0),
                        new DateTime(2023, 12, 15, 11, 0, 0),
                        new MailAddress(username),
                        attendees);
                    appointment.Summary = "Project Kickoff";
                    appointment.Description = "Discuss project scope and timeline.";
                    appointment.Location = "Conference Room";

                    // Set a reminder 15 minutes before the start
                    appointment.Reminders.Add(new AppointmentReminder());

                    // Synchronize the appointment to Exchange
                    string uid = client.CreateAppointment(appointment);
                    Console.WriteLine("Appointment created with UID: " + uid);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
