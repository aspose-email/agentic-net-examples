using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

public class Program
{
    public static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the EWS service URL and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a composite query:
                // - Subject contains "Report"
                // - From contains "alice@example.com"
                // - Sent date is within the last 30 days
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Report");
                builder.From.Contains("alice@example.com");
                builder.SentDate.Since(DateTime.Now.AddDays(-30));

                // Get the MailQuery object from the builder
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (non-recursive)
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Process and display the retrieved messages
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream
            Console.Error.WriteLine(ex.Message);
        }
    }
}