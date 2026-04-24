using Aspose.Email;
using System;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Gmail client credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Example: adjust a client property (e.g., timeout) – there is no direct method to update
                // settings such as default time format, so we demonstrate modifying available properties.
                gmailClient.Timeout = 120000; // Set timeout to 2 minutes.

                // Retrieve current settings.
                var settings = gmailClient.GetSettings();

                // Display some settings (for demonstration purposes).
                foreach (var kvp in settings)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                // If you need to modify a specific setting, you would typically use a dedicated API.
                // Since IGmailClient does not expose UpdateClientSettings, this step is omitted.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
