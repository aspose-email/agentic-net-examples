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
            // Placeholder credentials – skip execution in CI environments
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create an appointment with required properties
                Appointment appointment = new Appointment(
                    location: "Conference Room 112",
                    summary: "Team Sync",
                    description: "Weekly team synchronization meeting.",
                    startDate: DateTime.Now.AddDays(1).AddHours(9),
                    endDate: DateTime.Now.AddDays(1).AddHours(10),
                    organizer: new MailAddress("organizer@domain.com"),
                    attendees: attendees
                );

                // Additional optional properties
                appointment.Location = "Conference Room 112";
                appointment.Summary = "Team Sync";
                appointment.Description = "Weekly team synchronization meeting.";

                // Add the appointment to the Exchange calendar
                string appointmentUid = client.CreateAppointment(appointment);
                Console.WriteLine("Appointment created with UID: " + appointmentUid);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
