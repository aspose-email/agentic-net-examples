using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with placeholder credentials
            IGmailClient gmailClient;
            try
            {
                gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Define calendar identifier (use primary calendar)
            string calendarId = "primary";

            // Prepare organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Define appointment times
            DateTime start = new DateTime(2024, 5, 1, 10, 0, 0);
            DateTime end = new DateTime(2024, 5, 1, 11, 0, 0);

            // Create a daily recurrence pattern (every 1 day)
            RecurrencePattern recurrence = new DailyRecurrencePattern(1);

            // Create the appointment with recurrence
            Appointment appointment = new Appointment(
                "Conference Room A",
                "Team Sync",
                "Weekly team sync meeting",
                start,
                end,
                organizer,
                attendees,
                recurrence);

            // Create the appointment in the specified calendar
            try
            {
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine("Appointment created successfully.");
                Console.WriteLine($"Summary: {created.Summary}");
                Console.WriteLine($"Start: {created.StartDate}");
                Console.WriteLine($"End: {created.EndDate}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
