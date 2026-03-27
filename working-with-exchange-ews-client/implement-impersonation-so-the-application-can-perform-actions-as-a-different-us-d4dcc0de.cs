using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Service account mailbox URI and credentials
            string serviceMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential serviceCredentials = new NetworkCredential("serviceUser", "servicePassword", "DOMAIN");

            // Create EWS client for the service account
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceMailboxUri, serviceCredentials))
            {
                // Impersonate the target user
                string impersonatedUser = "impersonatedUser@example.com";
                ewsClient.ImpersonateUser(ItemChoice.PrimarySmtpAddress, impersonatedUser);

                // Build a query to find messages with a specific subject keyword
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Test");
                MailQuery query = builder.GetQuery();

                // List messages in the impersonated user's Inbox that match the query
                ExchangeMessageInfoCollection messages = ewsClient.ListMessages(ewsClient.MailboxInfo.InboxUri, query, false);
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch and display each message's subject
                    using (MailMessage message = ewsClient.FetchMessage(info.UniqueUri))
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