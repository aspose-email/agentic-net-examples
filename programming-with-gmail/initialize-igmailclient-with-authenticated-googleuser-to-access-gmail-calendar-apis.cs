using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder OAuth credentials – replace with real values for actual execution
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholder credentials are detected
            if (clientId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Gmail client credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client instance
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // List calendars accessible by the authenticated user
                    Calendar[] calendars = gmailClient.ListCalendars();
                    foreach (Calendar calendar in calendars)
                    {
                        Console.WriteLine($"Calendar ID: {calendar.Id}, Summary: {calendar.Summary}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing calendars: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
