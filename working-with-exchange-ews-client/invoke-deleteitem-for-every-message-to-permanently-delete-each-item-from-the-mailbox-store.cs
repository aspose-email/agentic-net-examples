using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize credentials and service URL (replace with actual values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // List all messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Delete each message permanently
                foreach (ExchangeMessageInfo info in messages)
                {
                    try
                    {
                        client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                        Console.WriteLine($"Deleted message: {info.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete message '{info.Subject}': {ex.Message}");
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
