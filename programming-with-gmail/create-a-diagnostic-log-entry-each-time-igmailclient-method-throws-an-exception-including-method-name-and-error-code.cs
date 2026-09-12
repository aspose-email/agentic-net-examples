using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailDiagnosticSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Skip live network calls when placeholders are detected.
                if (accessToken.StartsWith("YOUR_"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                // Create Gmail client instance safely.
                try
                {
                    using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                    {
                        // Example operation: list messages.
                        const string methodName = "ListMessages";
                        try
                        {
                            // This call may throw GoogleClientException.
                            System.Collections.Generic.List<GmailMessageInfo> messages = gmailClient.ListMessages();
                            Console.WriteLine($"Retrieved {messages.Count} messages.");
                        }
                        catch (GoogleClientException ex)
                        {
                            // Log method name and error code.
                            Console.Error.WriteLine($"Error in {methodName}: Code={ex.ErrorCode}, Message={ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            // Log unexpected exceptions.
                            Console.Error.WriteLine($"Unexpected error in {methodName}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle client creation failures.
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard.
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
