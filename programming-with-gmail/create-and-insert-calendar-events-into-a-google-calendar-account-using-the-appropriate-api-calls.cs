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
            // Initialize Gmail client with dummy OAuth credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            using (gmailClient)
            {
                // Define the calendar identifier (primary calendar)
                string calendarId = "primary";

                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@example.com"));
                attendees.Add(new MailAddress("person2@example.com"));
                attendees.Add(new MailAddress("person3@example.com"));

                // Organizer address
                MailAddress organizer = new MailAddress("organizer@example.com");

                // Create an appointment using a constructor that accepts all required details
                Appointment appointment = new Appointment(
                    "Conference Room",               // location
                    "Team Meeting",                  // summary
                    "Discuss project milestones",    // description
                    DateTime.Now.AddHours(1),        // start date/time
                    DateTime.Now.AddHours(2),        // end date/time
                    organizer,                       // organizer
                    attendees);                      // attendees

                // Insert the appointment into the specified Google Calendar
                Appointment created = gmailClient.CreateAppointment(calendarId, appointment);

                Console.WriteLine("Appointment created with ID: " + created.UniqueId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
