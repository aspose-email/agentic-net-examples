using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and user credentials
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client (returns IEWSClient)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // List messages in the Inbox folder
                var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (var info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}