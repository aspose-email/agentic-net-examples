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
            // Mailbox connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a query to filter messages from a specific domain and with "Report" in the subject
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.From.Contains("@example.com");
                builder.Subject.Contains("Report");
                MailQuery query = builder.GetQuery();

                // List messages in the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Output basic metadata
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + (info.From != null ? info.From.Address : "Unknown"));
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));

                    // Fetch the full message if further processing is needed
                    using (MailMessage fullMessage = client.FetchMessage(info.UniqueUri))
                    {
                        // Apply a content constraint: process only if the body contains "Confidential"
                        if (fullMessage.Body != null && fullMessage.Body.Contains("Confidential"))
                        {
                            Console.WriteLine("Processing confidential message: " + info.Subject);
                            // Insert custom processing logic here
                        }
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