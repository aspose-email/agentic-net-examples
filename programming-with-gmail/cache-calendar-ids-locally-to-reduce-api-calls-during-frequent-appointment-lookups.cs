using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Google;

class Program
{
    // Simple in‑memory cache for calendar appointments
    private static readonly Dictionary<string, Appointment[]> _appointmentCache = new Dictionary<string, Appointment[]>();

    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (accessToken == "YOUR_ACCESS_TOKEN" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client (variable name must be gmailClient)
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Example calendar identifier
                string calendarId = "primary";

                // Retrieve appointments using cache
                Appointment[] appointments = GetAppointments(gmailClient, calendarId);

                Console.WriteLine($"Found {appointments.Length} appointment(s) in calendar '{calendarId}'.");
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"- {appt.Summary} ({appt.StartDate} – {appt.EndDate})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Returns cached appointments or fetches them from Gmail if not cached
    private static Appointment[] GetAppointments(IGmailClient gmailClient, string calendarId)
    {
        if (_appointmentCache.TryGetValue(calendarId, out Appointment[] cached))
        {
            return cached;
        }

        try
        {
            Appointment[] fetched = gmailClient.ListAppointments(calendarId);
            _appointmentCache[calendarId] = fetched;
            return fetched;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
            return new Appointment[0];
        }
    }
}
