using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optional filter: subject contains a keyword if provided
                MailQuery query = null;
                if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                {
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains(args[0]);
                    query = builder.GetQuery();
                }

                // List messages from Inbox
                ExchangeMessageInfoCollection messages;
                if (query != null)
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                }
                else
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri);
                }

                // Iterate and display subjects
                foreach (ExchangeMessageInfo info in messages)
                {
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}