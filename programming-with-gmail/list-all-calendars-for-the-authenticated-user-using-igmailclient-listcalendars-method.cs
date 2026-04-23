using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run against Gmail.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholder credentials are detected.
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_"))
            {
                Console.WriteLine("Please provide valid Gmail OAuth credentials before running the sample.");
                return;
            }

            // Create the Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Retrieve all calendars for the authenticated user.
                    Calendar[] calendars = gmailClient.ListCalendars();

                    // Output basic information about each calendar.
                    foreach (Calendar calendar in calendars)
                    {
                        Console.WriteLine($"Calendar Id: {calendar.Id}, Summary: {calendar.Summary}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing calendars: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
