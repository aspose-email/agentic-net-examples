using Aspose.Email.Calendar;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Skip external call when placeholders are used
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Retrieve list of calendars
                    Calendar[] calendars = gmailClient.ListCalendars();

                    foreach (Calendar calendar in calendars)
                    {
                        Console.WriteLine($"Calendar ID: {calendar.Id}, Summary: {calendar.Summary}");

                        // Retrieve appointments for each calendar
                        Appointment[] appointments = gmailClient.ListAppointments(calendar.Id);

                        foreach (Appointment appointment in appointments)
                        {
                            Console.WriteLine($"  Appointment: {appointment.Summary}");
                            Console.WriteLine($"    Start: {appointment.StartDate}");
                            Console.WriteLine($"    End:   {appointment.EndDate}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving calendar items: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
