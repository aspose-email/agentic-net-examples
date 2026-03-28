using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsDeleteExample
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // Initialize connection to the Exchange server
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

                // Obtain an IEWSClient instance via the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Retrieve all messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate through each message and delete it permanently
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        try
                        {
                            client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                            Console.WriteLine($"Deleted message: {info.UniqueUri}");
                        }
                        catch (Exception ex)
                        {
                            // Log any deletion errors but continue processing remaining items
                            Console.Error.WriteLine($"Failed to delete {info.UniqueUri}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors that occur during setup or execution
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
