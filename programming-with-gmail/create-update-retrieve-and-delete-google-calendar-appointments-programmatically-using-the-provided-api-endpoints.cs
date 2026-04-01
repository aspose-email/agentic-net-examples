using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GmailCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values when running against a live account
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Skip execution if placeholder credentials are detected
                if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail calendar operations.");
                    return;
                }

                // Create Gmail client
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    try
                    {
                        // Retrieve the first calendar identifier
                        Calendar[] calendars = gmailClient.ListCalendars();
                        if (calendars == null || calendars.Length == 0)
                        {
                            Console.Error.WriteLine("No calendars found for the user.");
                            return;
                        }
                        string calendarId = calendars[0].Id;

                        // Prepare attendees for the appointment
                        MailAddressCollection attendees = new MailAddressCollection();
                        attendees.Add(new MailAddress("person1@example.com"));
                        attendees.Add(new MailAddress("person2@example.com"));

                        // Create a new appointment instance
                        Appointment newAppointment = new Appointment(
                            "Conference Room",
                            new DateTime(2024, 5, 20, 10, 0, 0),
                            new DateTime(2024, 5, 20, 11, 0, 0),
                            new MailAddress(defaultEmail),
                            attendees);
                        newAppointment.Summary = "Project Sync";
                        newAppointment.Description = "Discuss project status.";

                        // Create the appointment on Google Calendar
                        Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, newAppointment);
                        Console.WriteLine("Created appointment ID: " + createdAppointment.UniqueId);

                        // Retrieve the appointment by its identifier
                        Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                        Console.WriteLine("Fetched appointment summary: " + fetchedAppointment.Summary);

                        // Update the appointment's summary
                        fetchedAppointment.Summary = "Updated Project Sync";
                        Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                        Console.WriteLine("Updated appointment summary: " + updatedAppointment.Summary);

                        // Delete the appointment
                        gmailClient.DeleteAppointment(calendarId, updatedAppointment.UniqueId);
                        Console.WriteLine("Deleted appointment ID: " + updatedAppointment.UniqueId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Gmail operation error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
