using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string host = "example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls in CI
            if (host.Contains("example") || username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                "Team Sync",
                "Daily stand‑up meeting",
                new DateTime(2024, 4, 1, 9, 0, 0),
                new DateTime(2024, 4, 1, 9, 30, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Define a daily recurrence pattern (every day, 10 occurrences)
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(1);
            recurrence.Occurs = 10; // Number of occurrences
            // Alternatively, you could set recurrence.EndDate = new DateTime(2024, 4, 30);

            appointment.Recurrence = recurrence;

            // Connect to the Exchange server and add the appointment
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                try
                {
                    string uid = client.CreateAppointment(appointment);
                    Console.WriteLine($"Appointment created with UID: {uid}");
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
