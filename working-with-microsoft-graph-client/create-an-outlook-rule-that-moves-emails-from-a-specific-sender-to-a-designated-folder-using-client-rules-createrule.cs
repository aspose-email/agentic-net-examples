using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "yourPassword";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define the sender whose messages should be moved
                MailAddress senderAddress = new MailAddress("sender@example.com");

                // Destination folder identifier (replace with actual folder Id)
                string destinationFolderId = "destination-folder-id";

                // Create the inbox rule that moves messages from the specified sender
                InboxRule moveRule = InboxRule.CreateRuleMoveFrom(senderAddress, destinationFolderId);

                // Add the rule to the mailbox
                client.CreateInboxRule(moveRule);

                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
