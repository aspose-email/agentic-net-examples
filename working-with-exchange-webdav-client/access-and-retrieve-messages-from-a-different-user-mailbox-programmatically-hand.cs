using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange Web Services endpoint and admin credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("adminUser", "adminPass");

            // Create the EWS client (returns an IEWSClient instance)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Impersonate the target user whose mailbox we want to access
                client.ImpersonateUser(ItemChoice.SmtpAddress, "targetuser@example.com");

                // Retrieve the collection of messages from the target user's Inbox
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Fetch the full MailMessage for each item
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}