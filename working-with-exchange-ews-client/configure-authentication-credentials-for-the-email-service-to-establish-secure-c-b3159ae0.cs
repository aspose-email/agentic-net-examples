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

            // Create network credentials
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize the EWS client with the service URL and credentials
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // The client is now authenticated and ready for operations
                Console.WriteLine("EWS client authenticated successfully.");
                // Example operation: list messages in the Inbox folder
                // (Uncomment the following lines if you wish to retrieve messages)
                // var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                // foreach (var info in messages)
                // {
                //     Console.WriteLine($"Subject: {info.Subject}");
                // }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}