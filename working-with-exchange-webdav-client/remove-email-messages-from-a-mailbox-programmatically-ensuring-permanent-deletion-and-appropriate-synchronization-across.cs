using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Initialize the Exchange WebDav client (replace placeholders with real values)
            using (ExchangeClient client = new ExchangeClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                // Retrieve all messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                // Iterate through each message and delete it permanently
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Delete the message using its unique URI
                        client.DeleteMessage(messageInfo.UniqueUri);
                        Console.WriteLine($"Deleted message: {messageInfo.Subject}");
                    }
                    catch (Exception deleteEx)
                    {
                        // Log any deletion errors but continue processing other messages
                        Console.Error.WriteLine($"Failed to delete message '{messageInfo.Subject}': {deleteEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log any errors that occur during client initialization or processing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
