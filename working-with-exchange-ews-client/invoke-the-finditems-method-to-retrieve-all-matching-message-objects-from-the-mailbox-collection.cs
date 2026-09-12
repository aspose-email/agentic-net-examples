using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailFindItemsSample
{
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values before running against a live server.
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip external call when placeholders are still present.
            if (username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            try
            {
                // Create the EWS client.
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Build a query that matches all messages (no filter).
                    MailQuery query = new MailQueryBuilder().GetQuery();

                    // Retrieve all messages from the Inbox folder using ListMessages.
                    ExchangeMessageInfoCollection messageInfos = ewsClient.ListMessages("Inbox", query);

                    // Iterate over the retrieved message info objects.
                    foreach (ExchangeMessageInfo msgInfo in messageInfos)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                        Console.WriteLine($"From: {msgInfo.From}");
                        Console.WriteLine($"Received: {msgInfo.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during ListMessages operation: {ex.Message}");
            }
        }
    }
}
