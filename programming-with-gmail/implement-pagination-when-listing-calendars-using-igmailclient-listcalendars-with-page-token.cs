using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholder credentials are detected.
            if (accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // IGmailClient does not expose a paged ListCalendars method.
                // Retrieve all calendars in a single call.
                Calendar[] calendars = gmailClient.ListCalendars();

                Console.WriteLine($"Total calendars retrieved: {calendars.Length}");
                foreach (Calendar cal in calendars)
                {
                    Console.WriteLine($"- Id: {cal.Id}, Summary: {cal.Summary}");
                }

                // If a future version provides pagination via a page token,
                // the pattern would involve looping while a nextPageToken is returned.
                // Example (hypothetical):
                // string pageToken = null;
                // do
                // {
                //     var pageResult = gmailClient.ListCalendars(pageToken);
                //     foreach (var cal in pageResult.Calendars) { /* process */ }
                //     pageToken = pageResult.NextPageToken;
                // } while (!string.IsNullOrEmpty(pageToken));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
