using Aspose.Email;
using System;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";

            // Guard against placeholder values.
            if (clientId.Contains("YOUR_") ||
                clientSecret.Contains("YOUR_") ||
                refreshToken.Contains("YOUR_") ||
                defaultEmail.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid Gmail OAuth credentials.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Retrieve all calendars.
                    Calendar[] calendars = gmailClient.ListCalendars();

                    // Display metadata for each calendar.
                    foreach (Calendar calendar in calendars)
                    {
                        Console.WriteLine("Calendar ID: " + calendar.Id);
                        Console.WriteLine("Summary: " + calendar.Summary);
                        Console.WriteLine("Time Zone: " + calendar.TimeZone);
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while accessing Gmail calendars: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
