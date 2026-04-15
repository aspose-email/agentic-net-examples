using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client (replace with actual service URL and credentials)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Create (or get) a secondary calendar folder under the default Calendar folder
                    string parentFolderUri = client.MailboxInfo.CalendarUri;
                    ExchangeFolderInfo secondaryFolderInfo = client.CreateFolder(parentFolderUri, "MySecondaryCalendar", null, null);
                    string secondaryFolderUri = secondaryFolderInfo.Uri;

                    // Prepare attendees list
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    // Define a daily recurrence pattern (every day, 5 occurrences)
                    DailyRecurrencePattern recurrence = new DailyRecurrencePattern(DateTime.Today, 1);
                    recurrence.Occurs = 5;

                    // Create the appointment with location, summary, description, times, organizer, attendees, and recurrence
                    Appointment appointment = new Appointment(
                        "Conference Room A",                     // location
                        "Team Sync",                             // summary
                        "Weekly team sync meeting",              // description
                        new DateTime(2026, 5, 1, 10, 0, 0),      // start
                        new DateTime(2026, 5, 1, 11, 0, 0),      // end
                        new MailAddress("organizer@example.com"),// organizer
                        attendees,                               // attendees
                        recurrence);                                                 appointment.Summary = "Meeting Summary";
// recurrence pattern

                    // Create the appointment in the secondary calendar folder
                    string uid = client.CreateAppointment(appointment, secondaryFolderUri);
                    Console.WriteLine("Appointment created with UID: " + uid);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
