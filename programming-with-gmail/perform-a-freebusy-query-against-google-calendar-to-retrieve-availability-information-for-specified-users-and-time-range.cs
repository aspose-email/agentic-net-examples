using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using GoogleRange = Aspose.Email.Clients.Google.Range;

namespace AsposeEmailGmailFreeBusySample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with placeholder credentials
                IGmailClient gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");

                // Ensure the client is disposed properly
                using (gmailClient as IDisposable)
                {
                    // Define the time range for the free/busy query
                    DateTime timeMin = DateTime.UtcNow;
                    DateTime timeMax = timeMin.AddHours(8);

                    // List of calendar IDs to query
                    List<string> calendarIds = new List<string>
                    {
                        "primary",               // Example: primary calendar of the user
                        "othercalendar@example.com" // Example: another calendar ID
                    };

                    // Build the free/busy query
                    FreebusyQuery query = new FreebusyQuery(timeMin, timeMax, calendarIds);

                    // Retrieve free/busy information
                    FreebusyResponse response = gmailClient.GetFreebusyInfo(query);

                    // Process and display the results
                    foreach (KeyValuePair<string, FreebusyCalendarInfo> kvp in response.Calendars)
                    {
                        string calendarId = kvp.Key;
                        FreebusyCalendarInfo calendarInfo = kvp.Value;

                        Console.WriteLine($"Calendar ID: {calendarId}");

                        foreach (GoogleRange busyRange in calendarInfo.Busy)
                        {
                            Console.WriteLine($"  Busy from {busyRange.Start} to {busyRange.End}");
                        }

                        if (calendarInfo.Errors != null)
                        {
                            foreach (var error in calendarInfo.Errors)
                            {
                                Console.WriteLine($"  Error: {error}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
