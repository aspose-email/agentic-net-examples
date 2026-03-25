using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Zimbra server connection settings
            string host = "zimbra.example.com";
            int port = 993; // IMAP over SSL
            string username = "user@example.com";
            string password = "password";

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;
                client.EnableLogger = true;
                client.LogFileName = "imap_log.txt";

                // Connect to the server (connection is established lazily on first operation)
                // Subscribe to the "Updates" folder to receive product update notifications
                // This enables the subscription service for the specified channel.
                client.SubscribeFolder("Updates");

                Console.WriteLine("Subscription to 'Updates' folder enabled successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}