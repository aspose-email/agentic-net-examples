using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace with real credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the batch size for pagination
                const int batchSize = 50;

                // Retrieve messages from the Inbox folder in batches
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, batchSize);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message to access its properties
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine(message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
