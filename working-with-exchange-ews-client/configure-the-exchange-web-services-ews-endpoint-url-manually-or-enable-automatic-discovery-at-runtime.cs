using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace EwsConfigurationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // ----- Manual EWS endpoint configuration -----
                string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx"; // replace with actual EWS URL
                string username = "user@example.com";                       // replace with actual username
                string password = "password";                               // replace with actual password

                // Guard: skip external calls when placeholders are present
                if (ewsUrl.Contains("example.com") ||
                    username.Contains("example.com") ||
                    string.Equals(password, "password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping EWS call.");
                    return;
                }

                // Create network credentials
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Initialize the EWS client (IEWSClient) with the manual URL
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
                {
                    // Retrieve mailbox information
                    var mailboxInfo = client.GetMailboxInfo();

                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                }
            }
            catch (Exception ex)
            {
                // Global exception handling – write error and exit gracefully
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
