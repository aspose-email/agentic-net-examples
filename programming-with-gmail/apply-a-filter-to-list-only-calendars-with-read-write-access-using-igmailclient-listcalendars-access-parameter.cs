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

            // Guard against placeholder credentials to avoid live network calls during CI.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Please provide a valid OAuth 2.0 access token.");
                return;
            }

            // Create Gmail client.
            try
            {
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Retrieve calendars where the authenticated user has read‑write (writer) access.
                    try
                    {
                        Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars(AccessRole.writer, true);

                        if (calendars == null || calendars.Length == 0)
                        {
                            Console.WriteLine("No calendars with read‑write access were found.");
                            return;
                        }

                        Console.WriteLine("Calendars with read‑write access:");
                        foreach (Aspose.Email.Clients.Google.Calendar calendar in calendars)
                        {
                            Console.WriteLine($"- ID: {calendar.Id}, Summary: {calendar.Summary}");
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
                Console.Error.WriteLine($"Failed to create or connect Gmail client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
