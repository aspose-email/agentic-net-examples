using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailMeetingSample
{
    class Program
    {
        static void Main(string[] args)
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

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Define attendees (organizer and required room resource)
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("organizer@example.com"));
                    attendees.Add(new MailAddress("room1@example.com")); // required resource room

                    // Create the appointment
                    Appointment appointment = new Appointment(
                        "Conference Room 1",                                   // location
                        new DateTime(2023, 12, 1, 10, 0, 0),                  // start time
                        new DateTime(2023, 12, 1, 11, 0, 0),                  // end time
                        new MailAddress("organizer@example.com"),             // organizer
                        attendees);                                            // attendees

                    appointment.Summary = "Project Discussion";
                    appointment.Description = "Discuss project milestones and deliverables.";

                    // Send (create) the meeting request
                    string appointmentUid = client.CreateAppointment(appointment);
                    Console.WriteLine("Appointment created. UID: " + appointmentUid);

                    // Fetch the appointment to verify the room's acceptance status
                    string calendarUri = client.MailboxInfo.CalendarUri;
                    Appointment fetchedAppointment = client.FetchAppointment(appointmentUid, calendarUri);
                    Console.WriteLine("Fetched appointment status: " + fetchedAppointment.Status);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
