using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize IMAP client with Zimbra server credentials
            using (ImapClient imapClient = new ImapClient("zimbra.example.com", 993, "user@example.com", "password", SecurityOptions.SSLImplicit))
            {
                // Optional: enable logging to a file (ensure the directory exists)
                imapClient.EnableLogger = true;
                imapClient.LogFileName = "imap_log.txt";

                // Define the folder to monitor for product update notifications
                string folderToMonitor = "Inbox";

                // Start monitoring the specified folder
                imapClient.StartMonitoring(OnImapEvent, OnImapError, folderToMonitor);

                Console.WriteLine("Monitoring started. Press Enter to stop...");
                Console.ReadLine();

                // Stop monitoring before disposing the client
                imapClient.StopMonitoring();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Callback for handling new or deleted messages
    private static void OnImapEvent(object sender, ImapMonitoringEventArgs e)
    {
        try
        {
            Console.WriteLine($"Folder: {e.FolderName}");

            if (e.NewMessages != null && e.NewMessages.Length > 0)
            {
                foreach (ImapMessageInfo msgInfo in e.NewMessages)
                {
                    Console.WriteLine($"New message UID: {msgInfo.UniqueId}");
                    // Additional processing (e.g., fetch the message) can be added here
                }
            }

            if (e.DeletedMessages != null && e.DeletedMessages.Length > 0)
            {
                foreach (ImapMessageInfo msgInfo in e.DeletedMessages)
                {
                    Console.WriteLine($"Deleted message UID: {msgInfo.UniqueId}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Event handling error: " + ex.Message);
        }
    }

    // Callback for handling monitoring errors
    private static void OnImapError(object sender, ImapMonitoringErrorEventArgs e)
    {
        try
        {
            Console.Error.WriteLine($"Monitoring error in folder '{e.FolderName}': {e.Error.Message}");
            // Optionally, you can resume monitoring using e.MonitoringState
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error handling monitoring error: " + ex.Message);
        }
    }
}