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
            // Initialize EWS client with placeholder credentials
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(
                    "https://exchange.example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Iterate over each message info and fetch the full MailMessage
                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    MailMessage message;
                    try
                    {
                        message = client.FetchMessage(info.UniqueUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {info.UniqueUri}: {ex.Message}");
                        continue;
                    }

                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
