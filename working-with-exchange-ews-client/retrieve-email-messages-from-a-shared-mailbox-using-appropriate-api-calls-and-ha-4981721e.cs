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
            // Define the EWS endpoint and credentials for the shared mailbox
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method (returns an IEWSClient)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Retrieve the Inbox folder URI from the mailbox info
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List messages in the shared mailbox's Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    // Output basic metadata for each message
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
                    Console.Error.WriteLine("Error while accessing messages: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
        }
    }
}