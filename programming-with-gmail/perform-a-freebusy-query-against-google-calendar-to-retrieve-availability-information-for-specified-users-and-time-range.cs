using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace FreeBusySample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values to perform the query.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Guard against executing network calls with placeholder credentials.
                if (clientId == "clientId" ||
                    clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken" ||
                    defaultEmail == "user@example.com")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping free/busy query.");
                    return;
                }

                // Create Gmail client.
                IGmailClient gmailClient = null;
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
                    // Define the time range for the free/busy query.
                    DateTime timeMin = DateTime.UtcNow;
                    DateTime timeMax = timeMin.AddHours(2);

                    // List of calendar identifiers (email addresses) to query.
                    List<string> calendarIds = new List<string>
                    {
                        "user1@example.com",
                        "user2@example.com"
                    };

                    // Build the free/busy query.
                    FreebusyQuery query = new FreebusyQuery(timeMin, timeMax, calendarIds);

                    // Execute the query.
                    FreebusyResponse response = null;
                    try
                    {
                        response = gmailClient.GetFreebusyInfo(query);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to retrieve free/busy information: {ex.Message}");
                        return;
                    }

                    // Process and display the results.
                    if (response != null && response.Calendars != null)
                    {
                        foreach (KeyValuePair<string, FreebusyCalendarInfo> calendarEntry in response.Calendars)
                        {
                            Console.WriteLine($"Calendar ID: {calendarEntry.Key}");
                            FreebusyCalendarInfo calendarInfo = calendarEntry.Value;
                            if (calendarInfo != null && calendarInfo.Busy != null)
                            {
                                foreach (Aspose.Email.Clients.Google.Range busyRange in calendarInfo.Busy)
                                {
                                    Console.WriteLine($"  Busy from {busyRange.Start} to {busyRange.End}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("  No busy periods returned.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No free/busy data returned.");
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
