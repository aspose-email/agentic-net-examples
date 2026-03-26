using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox URI and credentials (dummy values for illustration)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Define the cutoff date (messages earlier than this date will be retrieved)
                    DateTime cutoffDate = new DateTime(2023, 1, 1);

                    // Build the query to find messages with SentDate earlier than the cutoff date
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    MailQuery dateQuery = builder.SentDate.Before(cutoffDate);
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox folder that match the query
                    string inboxFolderUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxFolderUri, query);

                    // Output basic information about each matching message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("Sent Date: " + info.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error output stream
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}