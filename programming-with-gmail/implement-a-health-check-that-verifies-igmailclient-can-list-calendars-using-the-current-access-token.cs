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

            // Skip execution when placeholders are detected to avoid external calls.
            if (accessToken == "YOUR_ACCESS_TOKEN" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail health check.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal.
            using (gmailClient as IDisposable)
            {
                try
                {
                    // Attempt to list calendars as a health check.
                    Calendar[] calendars = gmailClient.ListCalendars();

                    if (calendars != null && calendars.Length > 0)
                    {
                        Console.WriteLine("Gmail client health check passed. Calendars found:");
                        foreach (var cal in calendars)
                        {
                            Console.WriteLine($"- {cal.Id}: {cal.Summary}");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Gmail client health check failed: No calendars returned.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during calendar listing: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
