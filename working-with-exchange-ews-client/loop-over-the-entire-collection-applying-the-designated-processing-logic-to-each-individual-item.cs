using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a query (no filters, fetch all)
                MailQueryBuilder builder = new MailQueryBuilder();
                MailQuery query = builder.GetQuery();

                // List messages in the Inbox folder
                var messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                foreach (var info in messages)
                {
                    // Fetch the full message using its unique URI
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine("Subject: " + message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
