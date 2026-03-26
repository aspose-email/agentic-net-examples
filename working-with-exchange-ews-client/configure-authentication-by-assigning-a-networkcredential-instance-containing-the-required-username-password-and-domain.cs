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
            // Define the EWS service URL
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Create a NetworkCredential with username, password, and domain
            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Initialize the EWS client using the credentials
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // The client is now authenticated and ready for operations
                // Example: list messages in the Inbox (optional)
                // var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                // foreach (var info in messages)
                // {
                //     Console.WriteLine(info.Subject);
                // }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}