using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailInboxFetch
{
    class Program
    {
        static void Main()
        {
            // Replace with your actual EWS endpoint and credentials
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            try
            {
                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Get mailbox information to locate the Inbox folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // Retrieve all messages from the Inbox
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);

                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"Received: {messageInfo.InternalDate}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                // Gracefully handle any errors (connection, authentication, etc.)
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
