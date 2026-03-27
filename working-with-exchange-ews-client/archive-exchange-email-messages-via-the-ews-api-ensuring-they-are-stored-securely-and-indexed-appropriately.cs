using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the EWS endpoint and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Get the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List all messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    // Archive each message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        client.ArchiveItem(inboxUri, info.UniqueUri);
                        Console.WriteLine($"Archived message: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to connect to Exchange: {ex.Message}");
        }
    }
}