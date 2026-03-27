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
            // Replace with actual values or keep placeholders for demonstration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the EWS client safely
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                    Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    foreach (var info in messages)
                    {
                        Console.WriteLine($"---");
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received: {info.Date}");

                        // Fetch the full message to access additional details
                        using (MailMessage fullMessage = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine($"Body preview: {fullMessage.Body?.Substring(0, Math.Min(100, fullMessage.Body?.Length ?? 0))}");
                        }

                        // Example of deleting the message using the generic DeleteItem API
                        try
                        {
                            client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                            Console.WriteLine("Message deleted.");
                        }
                        catch (Exception deleteEx)
                        {
                            Console.Error.WriteLine($"Failed to delete message: {deleteEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
