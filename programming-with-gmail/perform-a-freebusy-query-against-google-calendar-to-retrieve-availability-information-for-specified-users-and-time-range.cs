using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailFreeBusyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client (replace placeholders with actual credentials)
                using (IGmailClient gmailClient = GmailClient.GetInstance("YOUR_ACCESS_TOKEN", "user@example.com"))
                {
                    // Define the time window for the free/busy query
                    DateTime timeMin = DateTime.UtcNow;
                    DateTime timeMax = timeMin.AddHours(24);

                    // Create a free/busy query for the specified calendar(s)
                    FreebusyQuery query = new FreebusyQuery(timeMin, timeMax, "user@example.com");

                    // Retrieve free/busy information
                    FreebusyResponse response = gmailClient.GetFreebusyInfo(query);

                    // Output the busy periods for each calendar
                    foreach (KeyValuePair<string, FreebusyCalendarInfo> calendarEntry in response.Calendars)
                    {
                        Console.WriteLine("Calendar ID: " + calendarEntry.Key);
                        foreach (Aspose.Email.Clients.Google.Range busyRange in calendarEntry.Value.Busy)
                        {
                            Console.WriteLine("  Busy from " + busyRange.Start + " to " + busyRange.End);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}