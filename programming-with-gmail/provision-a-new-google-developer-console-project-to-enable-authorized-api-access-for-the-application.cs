using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Replace these placeholders with actual credentials obtained from Google Developer Console.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Create a Gmail client instance using the provided OAuth credentials.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Example operation: retrieve and display the list of calendars.
                    Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                    Console.WriteLine("Calendars:");
                    foreach (Aspose.Email.Clients.Google.Calendar calendar in calendars)
                    {
                        Console.WriteLine($"- {calendar.Summary} (Id: {calendar.Id})");
                    }
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