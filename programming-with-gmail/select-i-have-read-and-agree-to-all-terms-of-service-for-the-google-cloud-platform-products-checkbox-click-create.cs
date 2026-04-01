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
            // Placeholder credentials – replace with real values when available.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid live network calls.
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

            // Use the client within a using block to ensure proper disposal.
            using (gmailClient)
            {
                try
                {
                    // Example operation: list calendars.
                    Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                    Console.WriteLine("Calendars retrieved:");
                    foreach (var calendar in calendars)
                    {
                        Console.WriteLine($"- {calendar.Id}: {calendar.Summary}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
