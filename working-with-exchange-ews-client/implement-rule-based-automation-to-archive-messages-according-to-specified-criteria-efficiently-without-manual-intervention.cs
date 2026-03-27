using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the EWS endpoint and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Archive messages older than 30 days
                foreach (ExchangeMessageInfo info in messages)
                {
                    if (info.Date < DateTime.Now.AddDays(-30))
                    {
                        // Move the message to the "Archive" folder
                        client.ArchiveItem(info.UniqueUri, "Archive");
                        Console.WriteLine($"Archived: {info.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
