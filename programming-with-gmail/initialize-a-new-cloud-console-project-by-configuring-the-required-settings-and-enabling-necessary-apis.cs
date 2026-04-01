using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials for the Google Cloud project
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard: skip real network calls when placeholders are used
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client initialization.");
                return;
            }

            // Initialize the Gmail client
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            using (gmailClient)
            {
                // Example operation: list calendars
                var calendars = gmailClient.ListCalendars();
                foreach (var calendar in calendars)
                {
                    Console.WriteLine($"Calendar ID: {calendar.Id}, Summary: {calendar.Summary}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
