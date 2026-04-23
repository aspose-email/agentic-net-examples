using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid real network calls.
            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                string.IsNullOrWhiteSpace(defaultEmail) ||
                clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Gmail client credentials are not set. Skipping network call.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

            // Ensure the client is disposed properly.
            using (gmailClient as IDisposable)
            {
                // Retrieve all client settings.
                Dictionary<string, string> settings = gmailClient.GetSettings();

                // Attempt to read specific settings.
                string language;
                string timeFormat;

                settings.TryGetValue("language", out language);
                settings.TryGetValue("timeFormat", out timeFormat);

                Console.WriteLine("Gmail Client Settings:");
                Console.WriteLine($"Language    : {(language ?? "Not found")}");
                Console.WriteLine($"Time Format : {(timeFormat ?? "Not found")}");

                // Optionally, list all settings for reference.
                Console.WriteLine("\nAll Settings:");
                foreach (KeyValuePair<string, string> entry in settings)
                {
                    Console.WriteLine($"{entry.Key} = {entry.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
