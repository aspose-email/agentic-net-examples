using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create the WebDAV Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                client.PreAuthenticate = true;

                // Define page size for pagination
                const int pageSize = 100;
                bool morePages = true;

                while (morePages)
                {
                    // Retrieve a page of messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, pageSize);

                    if (messages == null || messages.Count == 0)
                    {
                        morePages = false;
                        break;
                    }

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Date: {info.InternalDate}");
                        Console.WriteLine($"URI: {info.UniqueUri}");
                        Console.WriteLine(new string('-', 40));
                    }

                    // If fewer messages than pageSize were returned, we have reached the end
                    if (messages.Count < pageSize)
                    {
                        morePages = false;
                    }
                    else
                    {
                        // In a real scenario, adjust the request to fetch the next page.
                        // For this example we stop after the first page to avoid an infinite loop.
                        morePages = false;
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
