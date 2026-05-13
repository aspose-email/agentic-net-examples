using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace placeholders with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip real network calls when placeholders are used
            if (serviceUrl.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("attendee1@example.com"),
                    new MailAddress("attendee2@example.com")
                };

                // Create a recurring appointment (daily for demonstration)
                DateTime start = new DateTime(2024, 12, 1, 9, 0, 0);
                DateTime end = start.AddHours(1);
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Team Sync",
                    "Daily team sync meeting",
                    start,
                    end,
                    new MailAddress("organizer@example.com"),
                    attendees);

                appointment.Summary = "Meeting Summary";

                // Define recurrence pattern
                DailyRecurrencePattern recurrence = new DailyRecurrencePattern(start, 1)
                {
                    EndDate = start.AddMonths(1) // repeat for one month
                };
                appointment.Recurrence = recurrence;

                // Define exceptions (holidays) – stored locally for demonstration
                DateTime holiday = new DateTime(2024, 12, 25, 9, 0, 0);
                List<DateTime> exceptionDates = new List<DateTime> { holiday };

                // Create the appointment on the server
                string appointmentUid = client.CreateAppointment(appointment);

                // Fetch the appointment back (in a real scenario you would also fetch exceptions)
                Appointment fetched = client.FetchAppointment(appointmentUid, client.MailboxInfo.CalendarUri);

                // Simulate verification of exception dates
                Console.WriteLine("Verified exception dates (simulated):");
                foreach (DateTime exDate in exceptionDates)
                {
                    Console.WriteLine($"- Exception on {exDate:d}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
