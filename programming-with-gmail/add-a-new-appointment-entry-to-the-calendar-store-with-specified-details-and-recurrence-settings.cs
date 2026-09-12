using Aspose.Email.Calendar.Recurrences;
using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            // Author note: This sample demonstrates creating a recurring appointment in an Exchange calendar.
            try
            {
                // Exchange service connection details (replace with real values).
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the Exchange client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Prepare attendees.
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("person1@domain.com"));
                    attendees.Add(new MailAddress("person2@domain.com"));

                    // Define a daily recurrence pattern: every day, 5 occurrences.
                    DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(1);
                    dailyPattern.Occurs = 5; // Number of occurrences.

                    // Create the appointment with recurrence.
                    Appointment appointment = new Appointment(
                        location: "Conference Room",
                        summary: "Team Sync",
                        description: "Weekly sync meeting",
                        startDate: new DateTime(2023, 10, 1, 9, 0, 0),
                        endDate: new DateTime(2023, 10, 1, 10, 0, 0),
                        organizer: new MailAddress("organizer@domain.com"),
                        attendees: attendees,
                        recurrencePattern: dailyPattern);

                                        appointment.Summary = "Meeting Summary";
// Add the appointment to the default calendar folder.
                    string appointmentId = client.CreateAppointment(appointment);
                    Console.WriteLine($"Created appointment with ID: {appointmentId}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
