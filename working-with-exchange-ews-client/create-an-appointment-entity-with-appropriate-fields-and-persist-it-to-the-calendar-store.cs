using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailAppointmentSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service endpoint and credentials (replace with real values)
                string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve mailbox information to obtain the Calendar folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string calendarUri = mailboxInfo.CalendarUri;

                    // Prepare attendees
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("person1@domain.com"));
                    attendees.Add(new MailAddress("person2@domain.com"));
                    attendees.Add(new MailAddress("person3@domain.com"));

                    // Organizer address
                    MailAddress organizer = new MailAddress("organizer@domain.com");

                    // Create the appointment
                    Appointment appointment = new Appointment(
                        location: "Conference Room 1",
                        summary: "Project Kickoff",
                        description: "Discuss project goals and timelines.",
                        startDate: new DateTime(2024, 10, 1, 10, 0, 0),
                        endDate: new DateTime(2024, 10, 1, 11, 0, 0),
                        organizer: organizer,
                        attendees: attendees);

                                        appointment.Summary = "Meeting Summary";
// Persist the appointment to the Exchange calendar
                    string appointmentId = client.CreateAppointment(appointment, calendarUri);

                    Console.WriteLine("Appointment created successfully. ID: " + appointmentId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                // Gracefully exit without rethrowing
            }
        }
    }
}
