using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth credentials – replace with real values or retrieve from a secure source
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Validate placeholder values
            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.StartsWith("YOUR_"))
            {
                throw new InvalidOperationException("OAuth credentials contain placeholder values. Please replace them with actual credentials.");
            }

            // Create the Gmail client instance
            using IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

            // IDs of the calendars that should be permanently removed
            string[] calendarIds = new string[] { "calendarId1", "calendarId2" };

            foreach (string calendarId in calendarIds)
            {
                try
                {
                    // Delete the calendar
                    gmailClient.DeleteCalendar(calendarId);
                    Console.WriteLine($"Deleted calendar with ID: {calendarId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting calendar '{calendarId}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
