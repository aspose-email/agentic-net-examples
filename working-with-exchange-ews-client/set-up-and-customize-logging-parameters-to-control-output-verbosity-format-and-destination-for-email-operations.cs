using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;

namespace AsposeEmailLoggingExample
{
    class Program
    {
        static void Main()
        {
            // Input parameters
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "placeholder_user";
            string password = "placeholder_pass";
            string logFilePath = "ews_log.txt";

            // Prepare log file (ensure directory exists)
            try
            {
                string logDir = Path.GetDirectoryName(Path.GetFullPath(logFilePath));
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                // Start a fresh log for this run
                File.WriteAllText(logFilePath, $"Log started at {DateTime.Now}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log file: {ex.Message}");
                return;
            }

            // Detect placeholder credentials and skip network operations if present
            bool credentialsArePlaceholder = string.IsNullOrWhiteSpace(username) ||
                                             string.IsNullOrWhiteSpace(password) ||
                                             username.Contains("placeholder", StringComparison.OrdinalIgnoreCase) ||
                                             password.Contains("placeholder", StringComparison.OrdinalIgnoreCase);

            if (credentialsArePlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS client operations.");
                return;
            }

            // Helper local function for logging
            void Log(string message)
            {
                try
                {
                    File.AppendAllText(logFilePath, $"{DateTime.Now:O} - {message}{Environment.NewLine}");
                }
                catch
                {
                    // Swallow logging errors to avoid breaking main flow
                }
            }

            // Create and configure the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    Log("EWS client created successfully.");

                    // Example operation: retrieve mailbox information
                    var mailboxInfo = client.GetMailboxInfo();
                    Log($"Inbox URI: {mailboxInfo.InboxUri}");
                    Log($"Sent Items URI: {mailboxInfo.SentItemsUri}");

                    Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                    Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");
                }
            }
            catch (Exception ex)
            {
                string errMsg = $"EWS operation failed: {ex.Message}";
                Console.Error.WriteLine(errMsg);
                Log(errMsg);
            }
        }
    }
}
