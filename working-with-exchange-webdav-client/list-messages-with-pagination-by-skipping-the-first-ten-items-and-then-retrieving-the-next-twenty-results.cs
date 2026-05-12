using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string serviceUrl = "https://example.com/Exchange";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Define pagination: skip first 10 items, then retrieve next 20
                int skipCount = 10;
                int takeCount = 20;

                string inboxFolder = client.MailboxInfo.InboxUri;

                // Retrieve all messages from the inbox
                ExchangeMessageInfoCollection allMessages = client.ListMessages(inboxFolder);

                // Apply pagination
                var pagedMessages = allMessages
                                    .Skip(skipCount)
                                    .Take(takeCount);

                // Iterate through the retrieved messages
                foreach (ExchangeMessageInfo messageInfo in pagedMessages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
