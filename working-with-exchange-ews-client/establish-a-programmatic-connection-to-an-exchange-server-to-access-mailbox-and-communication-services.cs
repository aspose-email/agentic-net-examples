using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder values are detected to avoid unwanted network calls.
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create the EWS client inside a using block to ensure proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Retrieve mailbox information.
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // Display some mailbox URIs.
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);

                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    Console.WriteLine("Number of messages in Inbox: " + messages.Count);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Fetch the full message to access its subject.
                        MailMessage message = client.FetchMessage(messageInfo.UniqueUri);
                        Console.WriteLine("Subject: " + message.Subject);
                        // Dispose the fetched message.
                        message.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during Exchange operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
