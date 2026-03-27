using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy credentials
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

            // IDs of calendars that should be permanently removed
            string[] calendarsToDelete = new string[] { "calendarId1", "calendarId2" };

            foreach (string calendarId in calendarsToDelete)
            {
                try
                {
                    // Delete the calendar permanently
                    gmailClient.DeleteCalendar(calendarId);
                    Console.WriteLine($"Deleted calendar with ID: {calendarId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete calendar '{calendarId}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
