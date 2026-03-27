using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            try
            {
                // Create a new calendar
                Calendar calendar = new Calendar("Sample Calendar");
                string calendarId = gmailClient.CreateCalendar(calendar);

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("alice@example.com"));
                attendees.Add(new MailAddress("bob@example.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    "Conference Room A",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timelines.";

                // Insert the appointment into the created calendar
                Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);

                Console.WriteLine("Appointment created with ID: " + createdAppointment.UniqueId);
            }
            finally
            {
                // Ensure the client is disposed
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
