using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailCalendarDeleteSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace with valid OAuth 2.0 access token and default email address.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
                {
                    Console.WriteLine("Please provide a valid access token.");
                    return;
                }

                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    try
                    {
                        // Replace with the identifier of the calendar you want to delete.
                        string calendarId = "YOUR_CALENDAR_ID";

                        if (string.IsNullOrWhiteSpace(calendarId) || calendarId.StartsWith("YOUR_"))
                        {
                            Console.WriteLine("Please provide a valid calendar identifier.");
                            return;
                        }

                        gmailClient.DeleteCalendar(calendarId);
                        Console.WriteLine("Calendar deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error deleting calendar: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
