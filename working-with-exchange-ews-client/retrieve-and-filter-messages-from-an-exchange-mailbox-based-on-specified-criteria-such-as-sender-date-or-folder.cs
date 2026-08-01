using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace ExchangeMessageRetriever
{
    // Author: Aspose.Email example - retrieve and filter Exchange messages via EWS
    class Program
    {
        static void Main()
        {
            // Exchange server connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            try
            {
                // Create and dispose the EWS client safely
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Obtain mailbox folder URIs
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxFolder = mailboxInfo.InboxUri;

                    // Build a query to filter messages by sender and date (InternalDate)
                    // Adjust the sender email and date as needed
                    MailQuery query = new MailQuery("(('From' Contains 'sender@example.com') & 'InternalDate' >= '2023-01-01')");

                    // Retrieve matching messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxFolder, query);

                    // Output basic information for each message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"Received: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
