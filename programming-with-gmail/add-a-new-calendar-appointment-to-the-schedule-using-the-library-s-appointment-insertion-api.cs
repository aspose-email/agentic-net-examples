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
            // Initialize Gmail client with placeholder credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            try
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                // Create an appointment
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Project Kickoff",
                    "Discuss project goals and timeline",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Insert the appointment into the primary calendar
                Appointment created = gmailClient.CreateAppointment("primary", appointment);
                Console.WriteLine("Appointment created with ID: " + created.UniqueId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error while creating appointment: " + ex.Message);
                return;
            }
            finally
            {
                // Dispose the client if it implements IDisposable
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to initialize Gmail client: " + ex.Message);
        }
    }
}
