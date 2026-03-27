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
            // Mailbox connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the EWS client safely
            using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve all messages from the Inbox
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Iterate and delete selected messages permanently
                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                {
                    // Example condition: delete messages whose subject contains "DeleteMe"
                    if (info.Subject != null && info.Subject.Contains("DeleteMe"))
                    {
                        client.DeleteItem(info.UniqueUri, Aspose.Email.Clients.Exchange.WebService.DeletionOptions.DeletePermanently);
                        System.Console.WriteLine("Deleted: " + info.Subject);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Console.Error.WriteLine(ex.Message);
        }
    }
}