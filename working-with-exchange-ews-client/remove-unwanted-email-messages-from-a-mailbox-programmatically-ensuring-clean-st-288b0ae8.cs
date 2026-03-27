using System.Net;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
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

                // Create the EWS client using the factory method (returns IEWSClient)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve all messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Delete each message permanently
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}