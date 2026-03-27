using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with placeholder credentials.
            // Replace the placeholders with actual values when using the code.
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Get the Inbox folder URI.
                string inboxUri = client.MailboxInfo.InboxUri;

                // List all messages in the Inbox.
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Example filter: delete messages whose subject contains "Unwanted".
                    // Adjust the condition as needed.
                    if (info.Subject != null && info.Subject.IndexOf("Unwanted", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            // Delete the message permanently.
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
