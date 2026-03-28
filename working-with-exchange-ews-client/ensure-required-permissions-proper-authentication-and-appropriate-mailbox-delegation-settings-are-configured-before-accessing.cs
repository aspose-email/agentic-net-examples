using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services (EWS) endpoint and user credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Shared mailbox to access
                string sharedMailbox = "shared@example.com";

                // Verify that delegation is configured for the shared mailbox
                try
                {
                    var delegateUsers = client.ListDelegates(sharedMailbox);
                    if (delegateUsers == null || delegateUsers.Count == 0)
                    {
                        Console.Error.WriteLine("No delegation configured for the shared mailbox.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve delegation info: {ex.Message}");
                    return;
                }

                // Retrieve mailbox information for the shared mailbox
                try
                {
                    ExchangeMailboxInfo sharedInfo = client.GetMailboxInfo(sharedMailbox);
                    string inboxUri = sharedInfo.InboxUri;

                    // List messages in the shared mailbox's Inbox
                    var messageInfos = client.ListMessages(inboxUri);
                    foreach (var info in messageInfos)
                    {
                        // Fetch the full message using its unique URI
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing shared mailbox: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
