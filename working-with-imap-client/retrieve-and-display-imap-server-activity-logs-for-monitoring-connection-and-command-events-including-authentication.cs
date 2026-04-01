using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapActivityMonitor
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder IMAP server credentials
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing real network calls with placeholder data
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                    return;
                }

                // Prepare log file path and ensure its directory exists
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ImapLogs", "imap_activity.log");
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Subscribe to connection event for authentication monitoring
                    client.OnConnect += (sender, e) =>
                    {
                        Console.WriteLine("IMAP connection established and authenticated.");
                    };

                    // Define monitoring callbacks
                    ImapMonitoringEventHandler onChange = new ImapMonitoringEventHandler(OnImapChange);
                    ImapMonitoringErrorEventHandler onError = new ImapMonitoringErrorEventHandler(OnImapError);

                    try
                    {
                        // Start monitoring the INBOX folder
                        client.StartMonitoring(onChange, onError, "INBOX");
                        Console.WriteLine("Monitoring started. Press ENTER to stop...");

                        // Wait for user input to stop monitoring
                        Console.ReadLine();

                        // Stop monitoring
                        client.StopMonitoring();
                        Console.WriteLine("Monitoring stopped.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Callback for new/deleted messages
        private static void OnImapChange(object sender, ImapMonitoringEventArgs e)
        {
            Console.WriteLine($"Folder: {e.FolderName}");
            if (e.NewMessages != null && e.NewMessages.Length > 0)
            {
                Console.WriteLine($"New messages count: {e.NewMessages.Length}");
                foreach (ImapMessageInfo info in e.NewMessages)
                {
                    Console.WriteLine($"  New - UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
            if (e.DeletedMessages != null && e.DeletedMessages.Length > 0)
            {
                Console.WriteLine($"Deleted messages count: {e.DeletedMessages.Length}");
                foreach (ImapMessageInfo info in e.DeletedMessages)
                {
                    Console.WriteLine($"  Deleted - UID: {info.UniqueId}, Subject: {info.Subject}");
                }
            }
        }

        // Callback for monitoring errors
        private static void OnImapError(object sender, ImapMonitoringErrorEventArgs e)
        {
            Console.Error.WriteLine($"Monitoring error in folder '{e.FolderName}': {e.Error?.Message}");
        }
    }
}
