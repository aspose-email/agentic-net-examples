using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – avoid real network calls in CI
            string mailboxUri = "https://zimbra.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Zimbra configuration.");
                return;
            }

            // Initialize the Zimbra (EWS) client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Enable the subscription service (if applicable)
                    client.UpdateSubscription();

                    // Configure notification intervals (values are in minutes)
                    client.NotificationsCheckInterval = 5;   // check every 5 minutes
                    client.NotificationTimeout = 2;        // timeout after 2 minutes

                    Console.WriteLine("Zimbra subscription service configured successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error configuring Zimbra: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
