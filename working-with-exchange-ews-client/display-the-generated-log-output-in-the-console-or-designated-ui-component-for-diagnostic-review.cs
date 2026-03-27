using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
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
                // Define connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create credentials
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Initialize EWS client inside a using block
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Build a simple query (e.g., all messages)
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        MailQuery query = builder.GetQuery();

                        // List messages from the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                        // Output message subjects to the console
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("From: " + (info.From != null ? info.From.DisplayName : "Unknown"));
                            Console.WriteLine("Received: " + info.Date);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
                return;
            }
        }
    }
}