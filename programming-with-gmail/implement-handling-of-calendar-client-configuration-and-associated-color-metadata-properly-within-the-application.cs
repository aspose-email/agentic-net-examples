using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string userEmail = "user@example.com";

            // Skip network call when placeholders are used
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Initialize Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
            {
                try
                {
                    // Retrieve color metadata for calendars
                    ColorsInfo colorsInfo = gmailClient.GetColors();

                    // Output each calendar's color information
                    foreach (var entry in colorsInfo.Calendar)
                    {
                        string colorId = entry.Key;
                        Colors colors = entry.Value;
                        Console.WriteLine($"ColorId: {colorId}, Background: {colors.Background}, Foreground: {colors.Foreground}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail client operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
