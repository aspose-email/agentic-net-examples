using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailCreateCalendar
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values to run against Gmail.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                // Guard against executing network calls with placeholder credentials.
                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                    return;
                }

                // Create Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    try
                    {
                        // Define calendar details.
                        string summary = "Sample Calendar";
                        string description = "";
                        string location = "";
                        string timeZone = "America/New_York";

                        // Initialize Calendar object.
                        Calendar calendar = new Calendar(summary, description, location, timeZone);

                        // Create the calendar on Gmail.
                        string calendarId = gmailClient.CreateCalendar(calendar);
                        Console.WriteLine($"Created calendar with ID: {calendarId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during Gmail operations: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
