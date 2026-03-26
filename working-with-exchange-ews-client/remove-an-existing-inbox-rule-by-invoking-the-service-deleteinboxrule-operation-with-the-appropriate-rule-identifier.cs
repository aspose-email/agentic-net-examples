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
            NetworkCredential networkCredential = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, networkCredential))
            {
                try
                {
                    // Identifier of the inbox rule to delete
                    string ruleId = "rule-id-to-delete";

                    // Delete the specified inbox rule
                    client.DeleteInboxRule(ruleId);
                    Console.WriteLine("Inbox rule deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error deleting inbox rule: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error initializing EWS client: " + ex.Message);
        }
    }
}