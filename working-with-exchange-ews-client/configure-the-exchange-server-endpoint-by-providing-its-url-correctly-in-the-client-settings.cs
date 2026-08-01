using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: configure the Exchange Web Services (EWS) endpoint URL and credentials.
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Provide network credentials (username, password, domain).
            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Initialize the EWS client with the specified endpoint and credentials.
            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials);

            // Optional: adjust client settings, e.g., timeout.
            client.Timeout = 120000; // 2 minutes

            // Example operation: retrieve basic mailbox information.
            var mailboxInfo = client.GetMailboxInfo();
            Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
