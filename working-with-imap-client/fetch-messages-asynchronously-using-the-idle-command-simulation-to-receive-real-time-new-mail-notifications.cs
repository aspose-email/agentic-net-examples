using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP monitoring.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Ensure the client is connected by selecting a folder
                    client.SelectFolder("INBOX");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"Failed to connect or select folder: {imapEx.Message}");
                    return;
                }

                // Callback for new/deleted messages
                ImapMonitoringEventHandler onMonitoring = (object sender, ImapMonitoringEventArgs e) =>
                {
                    Console.WriteLine($"Monitoring event for folder: {e.FolderName}");

                    if (e.NewMessages != null && e.NewMessages.Length > 0)
                    {
                        Console.WriteLine("New messages:");
                        foreach (ImapMessageInfo info in e.NewMessages)
                        {
                            Console.WriteLine($"  UID: {info.UniqueId}");
                        }
                    }

                    if (e.DeletedMessages != null && e.DeletedMessages.Length > 0)
                    {
                        Console.WriteLine("Deleted messages:");
                        foreach (ImapMessageInfo info in e.DeletedMessages)
                        {
                            Console.WriteLine($"  UID: {info.UniqueId}");
                        }
                    }
                };

                // Callback for monitoring errors
                ImapMonitoringErrorEventHandler onError = (object sender, ImapMonitoringErrorEventArgs e) =>
                {
                    string errorMessage = e.Error != null ? e.Error.Message : "Unknown error";
                    Console.Error.WriteLine($"Monitoring error in folder '{e.FolderName}': {errorMessage}");
                };

                // Start asynchronous monitoring (IDLE simulation)
                await client.StartMonitoringAsync(onMonitoring, onError, "INBOX");

                // Keep the application running to receive events
                Console.WriteLine("Press Enter to stop monitoring...");
                Console.ReadLine();

                // Stop monitoring gracefully
                client.StopMonitoringAsync("INBOX").Wait();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
