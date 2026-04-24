using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and calendar identifier.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";
            string calendarId = "YOUR_CALENDAR_ID";

            // Skip execution when placeholders are present to avoid real network calls.
            if (accessToken.StartsWith("YOUR_") ||
                defaultEmail.StartsWith("YOUR_") ||
                calendarId.StartsWith("YOUR_"))
            {
                Console.WriteLine("Please replace placeholder values with valid credentials and calendar ID.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Fetch the existing calendar.
                    ExtendedCalendar calendar = gmailClient.FetchCalendar(calendarId);

                    // Update description and background color.
                    calendar.Description = "Updated calendar description.";
                    calendar.BackgroundColor = "#ff0000"; // Example color in hex format.

                    // Apply the updates.
                    gmailClient.UpdateCalendar(calendar);

                    Console.WriteLine("Calendar updated successfully.");
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
