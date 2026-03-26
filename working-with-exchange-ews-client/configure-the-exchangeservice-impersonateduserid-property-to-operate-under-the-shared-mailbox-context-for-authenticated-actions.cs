using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailImpersonationExample
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
                string sharedMailboxAddress = "sharedmailbox@example.com";

                // Create network credentials
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Initialize EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Impersonate the shared mailbox user
                        client.ImpersonateUser(ItemChoice.SmtpAddress, sharedMailboxAddress);

                        // Build a simple query (optional, can be omitted to list all messages)
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Test");
                        MailQuery query = builder.GetQuery();

                        // List messages from the impersonated mailbox's Inbox
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                        // Output basic information about each message
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("From: " + info.From);
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
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}