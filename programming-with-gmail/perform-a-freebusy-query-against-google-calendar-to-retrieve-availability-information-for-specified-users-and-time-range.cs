using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // ----- Configuration (replace with real values) -----
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";

            // Guard against placeholder values
            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder configuration values with real credentials.");
                return;
            }

            // ----- Obtain OAuth access token -----
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
            OAuthToken oauthToken = tokenProvider.GetAccessToken();
            string accessToken = oauthToken.Token;

            // ----- Create Gmail client -----
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Define the time range for the free/busy query
                    DateTime start = DateTime.UtcNow;
                    DateTime end = start.AddHours(8);

                    // List of calendar IDs (email addresses) to query
                    string[] calendars = new string[] { "user1@example.com", "user2@example.com" };

                    // Build the query
                    FreebusyQuery query = new FreebusyQuery(start, end, calendars);

                    // Execute the query
                    FreebusyResponse response = gmailClient.GetFreebusyInfo(query);

                    // Process and display the results
                    if (response != null && response.Calendars != null)
                    {
                        foreach (KeyValuePair<string, FreebusyCalendarInfo> kvp in response.Calendars)
                        {
                            FreebusyCalendarInfo calendarInfo = kvp.Value;
                            Console.WriteLine($"Calendar: {calendarInfo.CalendarId}");

                            // Print any errors associated with this calendar
                            if (calendarInfo.Errors != null)
                            {
                                foreach (ErrorDetails err in calendarInfo.Errors)
                                {
                                    Console.WriteLine($"  Error: {err}");
                                }
                            }

                            // Print busy time ranges
                            if (calendarInfo.Busy != null)
                            {
                                foreach (Aspose.Email.Clients.Google.Range busyRange in calendarInfo.Busy)
                                {
                                    Console.WriteLine($"  Busy from {busyRange.Start} to {busyRange.End}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No free/busy information returned.");
                    }
                }
                catch (GoogleClientException gex)
                {
                    Console.Error.WriteLine($"Google client error (code {gex.ErrorCode}): {gex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
