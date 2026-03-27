using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client via the factory method (returns IEWSClient)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Retrieve all messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Permanently delete each message
                foreach (ExchangeMessageInfo info in messages)
                {
                    client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine($"Deleted message: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}