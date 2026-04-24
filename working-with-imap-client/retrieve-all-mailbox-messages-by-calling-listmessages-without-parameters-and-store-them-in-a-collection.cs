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
            // Placeholder credentials – replace with real values for actual execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip network call when placeholders are detected.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operation.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            try
            {
                // Retrieve all messages from the mailbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                // Example usage: output count of retrieved messages.
                Console.WriteLine($"Retrieved {messages.Count} messages from Gmail mailbox.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while listing Gmail messages: {ex.Message}");
                return;
            }
            finally
            {
                // Ensure the client is properly disposed.
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
