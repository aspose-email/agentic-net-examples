using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to execute the call
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string userEmail = "user@example.com";

            // Skip external call when placeholders are used
            if (clientId == "clientId")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                // Create appointment instance
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Team Meeting",
                    "Discuss project status",
                    new DateTime(2023, 10, 15, 10, 0, 0),
                    new DateTime(2023, 10, 15, 11, 0, 0),
                    new MailAddress(userEmail),
                    attendees);

                // Calendar identifier (use "primary" for the main calendar)
                string calendarId = "primary";

                // Insert the appointment into the calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine("Appointment created with ID: " + created.UniqueId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
