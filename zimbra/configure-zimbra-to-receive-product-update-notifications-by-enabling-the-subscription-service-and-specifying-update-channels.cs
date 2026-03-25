using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Zimbra EWS endpoint and user credentials
            string mailboxUri = "https://zimbra.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client (disposable)
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Enable periodic notification checks (in seconds)
                ewsClient.NotificationsCheckInterval = 30; // check every 30 seconds

                // Set the timeout for each notification request (in seconds)
                ewsClient.NotificationTimeout = 120; // 2 minutes

                // Optionally, configure folder separator usage
                ewsClient.UseSlashAsFolderSeparator = true;

                // The client is now configured to receive notifications.
                // In a real scenario you would call methods such as SyncFolder or
                // monitor events to process product update messages.
                Console.WriteLine("Subscription service enabled for Zimbra. Listening for product update notifications...");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}