using Aspose.Email.Clients;
using System;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – avoid real network calls in CI.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping real connection.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Start monitoring the INBOX folder for new messages.
                    client.StartMonitoring(OnNewMail, OnMonitoringError, "INBOX");

                    Console.WriteLine("Monitoring started. Press Enter to stop...");
                    Console.ReadLine(); // Keep the application alive until user decides to stop.
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
                finally
                {
                    // Ensure monitoring is stopped before disposing the client.
                    try
                    {
                        client.StopMonitoring("INBOX");
                    }
                    catch
                    {
                        // Ignored – best effort cleanup.
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }

    // Callback invoked when new messages are detected.
    private static void OnNewMail(object sender, ImapMonitoringEventArgs e)
    {
        foreach (ImapMessageInfo newMessage in e.NewMessages)
        {
            Console.WriteLine($"New message detected - UID: {newMessage.UniqueId}, Subject: {newMessage.Subject}");
        }
    }

    // Callback invoked when a monitoring error occurs.
    private static void OnMonitoringError(object sender, ImapMonitoringErrorEventArgs e)
    {
        Console.Error.WriteLine($"Monitoring error: {e.Error.Message}");
    }
}
