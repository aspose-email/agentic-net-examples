using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

public class Program
{
    public static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values when available
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Skip execution if placeholder credentials are detected
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client (IDisposable) and ensure it is disposed
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Define the calendar IDs that should be permanently deleted
                string[] calendarIds = new string[] { "calendarId1", "calendarId2" };

                foreach (string calendarId in calendarIds)
                {
                    try
                    {
                        // Delete the specified calendar
                        gmailClient.DeleteCalendar(calendarId);
                        Console.WriteLine($"Deleted calendar with ID: {calendarId}");
                    }
                    catch (Exception ex)
                    {
                        // Log any errors that occur while deleting a calendar
                        Console.Error.WriteLine($"Failed to delete calendar {calendarId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception handling
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
