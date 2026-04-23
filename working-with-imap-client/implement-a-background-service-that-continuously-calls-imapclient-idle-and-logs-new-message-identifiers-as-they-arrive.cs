using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping monitoring.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Start monitoring the Inbox folder for new messages
                    Task monitoringTask = client.StartMonitoringAsync(OnNewMessage, OnMonitoringError, "Inbox");

                    // Keep the application running indefinitely
                    await Task.Delay(Timeout.Infinite);
                    await monitoringTask; // This line is never reached but keeps the compiler happy
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }

    // Callback invoked when new messages are detected
    private static void OnNewMessage(object sender, ImapMonitoringEventArgs e)
    {
        foreach (ImapMessageInfo messageInfo in e.NewMessages)
        {
            Console.WriteLine($"New message UID: {messageInfo.UniqueId}");
        }
    }

    // Callback invoked when a monitoring error occurs
    private static void OnMonitoringError(object sender, ImapMonitoringErrorEventArgs e)
    {
        Console.Error.WriteLine($"Monitoring error in folder '{e.FolderName}': {e.Error.Message}");
    }
}
