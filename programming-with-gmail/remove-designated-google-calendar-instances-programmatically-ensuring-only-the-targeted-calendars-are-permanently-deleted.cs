using System;
using System.Collections.Generic;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace DeleteGoogleCalendars
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with dummy OAuth credentials
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                IGmailClient gmailClient;
                try
                {
                    gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                using (gmailClient)
                {
                    // List of calendar identifiers to delete
                    List<string> calendarsToDelete = new List<string>
                    {
                        "calendarId1",
                        "calendarId2"
                    };

                    foreach (string calendarId in calendarsToDelete)
                    {
                        try
                        {
                            // Permanently delete the specified calendar
                            gmailClient.DeleteCalendar(calendarId);
                            Console.WriteLine($"Deleted calendar with ID: {calendarId}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete calendar {calendarId}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
