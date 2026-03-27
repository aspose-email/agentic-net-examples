using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the Exchange Web Services (EWS) mailbox URI
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Set the credentials for authentication
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client and establish a connection
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Connection is established by the factory method
                Console.WriteLine("Connected to Exchange server.");

                // Retrieve mailbox information to verify the connection
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
