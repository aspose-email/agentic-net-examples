using System;
using System.Net;
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
            // Mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Attempt to create an "Archive" folder under the Inbox
                try
                {
                    client.CreateFolder("Archive", client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    // Folder may already exist; log warning and continue
                    Console.Error.WriteLine($"Warning: could not create folder – {ex.Message}");
                }

                // Define keywords for the rule
                string[] keywords = new string[] { "spam", "advertisement" };

                // Create an inbox rule that moves matching messages to the Archive folder
                InboxRule rule = InboxRule.CreateRuleMoveContaining(keywords, "Archive");

                // Apply the rule on the server
                client.CreateInboxRule(rule);

                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }
}