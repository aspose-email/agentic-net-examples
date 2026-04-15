using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials for the authenticated user
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Email address of the mailbox to access
                    string otherUserEmail = "otheruser@example.com";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || otherUserEmail.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Retrieve mailbox information for the specified user
                    ExchangeMailboxInfo otherMailboxInfo = client.GetMailboxInfo(otherUserEmail);

                    // Display selected mailbox URIs
                    Console.WriteLine("Inbox URI: " + otherMailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + otherMailboxInfo.SentItemsUri);
                    Console.WriteLine("Drafts URI: " + otherMailboxInfo.DraftsUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to get mailbox info: " + ex.Message);
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
