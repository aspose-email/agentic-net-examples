using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

public class Program
{
    public static void Main()
    {
        try
        {
            // Connection settings (replace with real credentials)
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Cutoff date: retrieve messages dated earlier than this
            DateTime cutoffDate = new DateTime(2023, 1, 1);

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Build a simple query string for messages before the cutoff date
                string queryString = $"InternalDate < '{cutoffDate:yyyy-MM-dd}'";
                MailQuery query = new MailQuery(queryString);

                // Get the Inbox folder URI from the mailbox info
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                // Process the filtered messages
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    Console.WriteLine($"Subject: {msgInfo.Subject}, Received: {msgInfo.InternalDate}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
