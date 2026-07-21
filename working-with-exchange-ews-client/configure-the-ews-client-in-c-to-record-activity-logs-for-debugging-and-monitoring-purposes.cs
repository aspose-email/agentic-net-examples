using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Example configuration for EWS client logging.
            const string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            const string username = "user@example.com";
            const string password = "password";

            // Guard: skip real network calls when placeholder values are detected.
            if (IsPlaceholder(mailboxUri, username, password))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS client initialization.");
                return;
            }

            // Define log file path and ensure its directory exists.
            string logFilePath = Path.Combine("logs", "ews_activity.log");
            string logDir = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            // Create and configure the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true;

                // Example operation to generate log entries (optional).
                // var mailboxInfo = client.GetMailboxInfo();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static bool IsPlaceholder(string uri, string user, string pass)
    {
        return uri.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
               user.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(pass, "password", StringComparison.Ordinal);
    }
}
