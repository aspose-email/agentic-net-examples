using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapIdleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                    return;
                }

                // Timeout for idle monitoring (in milliseconds)
                int idleTimeout = 30000; // 30 seconds

                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Start monitoring for new messages in the Inbox folder
                    ImapMonitoringEventHandler newMessageHandler = (object sender, ImapMonitoringEventArgs e) =>
                    {
                        ImapMessageInfo[] newMessages = e.NewMessages;
                        if (newMessages != null)
                        {
                            foreach (ImapMessageInfo messageInfo in newMessages)
                            {
                                Console.WriteLine($"New message received: Subject = {messageInfo.Subject}");
                            }
                        }
                    };

                    ImapMonitoringErrorEventHandler errorHandler = (object sender, ImapMonitoringErrorEventArgs e) =>
                    {
                        Console.Error.WriteLine($"Monitoring error: {e}");
                    };

                    client.StartMonitoring(newMessageHandler, errorHandler, "Inbox");

                    // Wait for the specified timeout period
                    Thread.Sleep(idleTimeout);

                    // Stop monitoring after timeout
                    client.StopMonitoring();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
