using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters for the Zimbra server (EWS endpoint)
            string mailboxUri = "https://zimbra.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Enable the subscription service by configuring check interval and timeout
                client.NotificationsCheckInterval = 300; // seconds between checks
                client.NotificationTimeout = 60;        // seconds to wait for a response

                // Specify the update channels (e.g., Inbox, Calendar, Contacts) via a custom header
                client.AddHeader("X-Update-Channels", "Inbox,Calendar,Contacts");

                // Apply the subscription settings
                client.UpdateSubscription();

                Console.WriteLine("Zimbra subscription service configured successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
