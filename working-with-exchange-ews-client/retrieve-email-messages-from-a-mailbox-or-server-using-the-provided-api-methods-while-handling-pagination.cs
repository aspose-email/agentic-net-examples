using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // Pagination settings
                int pageSize = 50;
                int pageNumber = 0;
                bool morePages = true;

                while (morePages)
                {
                    // Retrieve a page of messages
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, pageSize);

                    if (messages == null || messages.Count == 0)
                    {
                        morePages = false;
                        break;
                    }

                    Console.WriteLine($"Page {pageNumber + 1}:");
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received: {info.Date}");
                        Console.WriteLine(new string('-', 40));
                    }

                    // If fewer messages than the page size were returned, we are at the end
                    if (messages.Count < pageSize)
                    {
                        morePages = false;
                    }
                    else
                    {
                        // In a real scenario, adjust the query to fetch the next set.
                        // For this example, we stop after the first page to avoid an infinite loop.
                        morePages = false;
                    }

                    pageNumber++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}