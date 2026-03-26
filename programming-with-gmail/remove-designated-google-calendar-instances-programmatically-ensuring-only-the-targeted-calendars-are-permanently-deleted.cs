using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Gmail client (replace placeholders with real credentials)
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string userEmail = "user@example.com";

            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail);

            // Calendars that should be removed
            string[] targetCalendarNames = new string[] { "Test Calendar", "Old Calendar" };

            // Retrieve all calendars for the user
            Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();

            foreach (Aspose.Email.Clients.Google.Calendar calendar in calendars)
            {
                foreach (string targetName in targetCalendarNames)
                {
                    if (calendar.Summary.Equals(targetName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Permanently delete the matching calendar
                        gmailClient.DeleteCalendar(calendar.Id);
                        Console.WriteLine($"Deleted calendar: {calendar.Summary} (Id: {calendar.Id})");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}