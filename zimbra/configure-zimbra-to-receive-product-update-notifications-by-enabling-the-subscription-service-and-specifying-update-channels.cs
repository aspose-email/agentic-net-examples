using System.Collections.Generic;
using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

public class Program
{
    public static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            // Zimbra server connection details
            string host = "zimbra.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Start monitoring the Inbox folder for updates
                    imapClient.StartMonitoring(OnImapEvent, OnImapError, "Inbox");
                    Console.WriteLine("Monitoring started. Press Enter to stop...");
                    Console.ReadLine();

                    // Stop monitoring before disposing the client
                    imapClient.StopMonitoring();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Monitoring error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }

    // Callback for handling new and deleted messages
    private static void OnImapEvent(object sender, ImapMonitoringEventArgs e)
    {
        try
        {
            if (e.NewMessages != null && e.NewMessages.Length > 0)
            {
                Console.WriteLine("New messages detected in folder: " + e.FolderName);
                foreach (ImapMessageInfo info in e.NewMessages)
                {
                    Console.WriteLine("New message UID: " + info.UniqueId);
                }
            }

            if (e.DeletedMessages != null && e.DeletedMessages.Length > 0)
            {
                Console.WriteLine("Deleted messages detected in folder: " + e.FolderName);
                foreach (ImapMessageInfo info in e.DeletedMessages)
                {
                    Console.WriteLine("Deleted message UID: " + info.UniqueId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error in event handler: " + ex.Message);
        }
    }

    // Callback for handling monitoring errors
    private static void OnImapError(object sender, ImapMonitoringErrorEventArgs e)
    {
        try
        {
            Console.Error.WriteLine("Monitoring error in folder: " + e.FolderName);
            Console.Error.WriteLine("Error: " + e.Error.Message);

            // Attempt to stop monitoring for the problematic folder
            if (sender is ImapClient client)
            {
                client.StopMonitoring(e.FolderName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error in error handler: " + ex.Message);
        }
    }
}