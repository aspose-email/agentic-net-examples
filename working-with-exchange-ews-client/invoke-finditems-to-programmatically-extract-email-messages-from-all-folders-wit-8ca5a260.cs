using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // EWS endpoint and credentials for the shared mailbox
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "shared_user@example.com";
            string password = "password";

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve mailbox information (contains folder URIs)
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // List all messages in the Inbox and its subfolders recursively
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri, true);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Fetch the full message to access its properties
                    MailMessage fullMessage = client.FetchMessage(messageInfo.UniqueUri);
                    Console.WriteLine("Subject: " + fullMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}