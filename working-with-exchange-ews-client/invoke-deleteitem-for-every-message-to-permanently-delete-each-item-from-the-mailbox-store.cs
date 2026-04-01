using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – in real scenarios replace with actual values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder data.
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // List all messages in the default inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages();

                // Delete each message permanently.
                foreach (ExchangeMessageInfo info in messages)
                {
                    // DeletionOptions can be customized if needed; default performs permanent delete.
                    DeletionOptions options = new DeletionOptions();
                    client.DeleteItem(info.UniqueUri, options);
                }

                Console.WriteLine("All messages have been permanently deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
