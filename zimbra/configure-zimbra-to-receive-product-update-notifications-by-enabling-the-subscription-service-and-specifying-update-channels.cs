using System.Collections.Generic;
using Aspose.Email.Clients;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace ZimbraSubscriptionExample
{
    class Program
    {
        // Callback for successful monitoring events
        private static void OnImapMonitoring(object sender, ImapMonitoringEventArgs e)
        {
            Console.WriteLine("Monitoring event for folder: " + e.FolderName);
            if (e.NewMessages != null && e.NewMessages.Length > 0)
            {
                Console.WriteLine("New messages count: " + e.NewMessages.Length);
            }
            if (e.DeletedMessages != null && e.DeletedMessages.Length > 0)
            {
                Console.WriteLine("Deleted messages count: " + e.DeletedMessages.Length);
            }
        }

        // Callback for monitoring errors
        private static void OnImapMonitoringError(object sender, ImapMonitoringErrorEventArgs e)
        {
            Console.Error.WriteLine("Monitoring error for folder: " + e.FolderName);
            if (e.Error != null)
            {
                Console.Error.WriteLine("Error: " + e.Error.Message);
            }
        }

        static void Main(string[] args)
        {
        List<MailMessage> messages = new List<MailMessage>();

            try
            {
                // Configuration parameters (replace with actual values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    client.EnableLogger = true;
                    client.LogFileName = "imap_log.txt";

                    // Ensure the folder we want to monitor exists
                    try
                    {
                        client.SelectFolder("INBOX");
                    }
                    catch (Exception folderEx)
                    {
                        Console.Error.WriteLine("Failed to select folder INBOX: " + folderEx.Message);
                        return;
                    }

                    // Subscribe to the folder (enables subscription service)
                    try
                    {
                        client.SubscribeFolderAsync("INBOX").GetAwaiter().GetResult();
                        Console.WriteLine("Subscribed to INBOX folder.");
                    }
                    catch (Exception subEx)
                    {
                        Console.Error.WriteLine("Failed to subscribe to folder: " + subEx.Message);
                        // Continue even if subscription fails; monitoring may still work
                    }

                    // Start monitoring for product update notifications (example channel)
                    try
                    {
                        client.StartMonitoring(OnImapMonitoring, OnImapMonitoringError, "INBOX");
                        Console.WriteLine("Started monitoring INBOX for updates. Press Enter to stop.");
                        Console.ReadLine();
                        client.StopMonitoring();
                    }
                    catch (Exception monitorEx)
                    {
                        Console.Error.WriteLine("Monitoring failed: " + monitorEx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}