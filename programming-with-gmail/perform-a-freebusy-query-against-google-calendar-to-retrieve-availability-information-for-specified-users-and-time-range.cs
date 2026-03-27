using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using RangeAlias = Aspose.Email.Clients.Google.Range;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy OAuth credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                clientId: "clientId",
                clientSecret: "clientSecret",
                refreshToken: "refreshToken",
                defaultEmail: "user@example.com");

            using (gmailClient)
            {
                // Define the time window for the free/busy query
                DateTime timeMin = DateTime.UtcNow;
                DateTime timeMax = timeMin.AddHours(2);

                // List of calendar identifiers (email addresses) to query
                string[] calendarIds = new string[] { "user1@example.com", "user2@example.com" };

                // Build the free/busy request
                FreebusyQuery query = new FreebusyQuery(timeMin, timeMax, calendarIds);

                // Execute the query
                FreebusyResponse response = gmailClient.GetFreebusyInfo(query);

                // Output the busy periods for each calendar
                foreach (KeyValuePair<string, FreebusyCalendarInfo> entry in response.Calendars)
                {
                    Console.WriteLine($"Calendar ID: {entry.Key}");
                    foreach (RangeAlias busy in entry.Value.Busy)
                    {
                        Console.WriteLine($"  Busy from {busy.Start:u} to {busy.End:u}");
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
