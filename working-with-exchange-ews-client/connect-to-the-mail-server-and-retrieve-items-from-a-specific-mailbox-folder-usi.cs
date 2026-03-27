using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the EWS service URL and user credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Verify that the client can access mailbox information.
                try
                {
                    Aspose.Email.Clients.Exchange.ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve mailbox info: {ex.Message}");
                    return;
                }

                // Get the URI of the Inbox folder.
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}