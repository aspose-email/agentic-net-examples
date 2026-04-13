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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and dispose the EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Use the Inbox folder for paging
                string folderUri = client.MailboxInfo.InboxUri;

                const int itemsPerPage = 50;
                int offset = 0;
                bool lastPage = false;

                while (!lastPage)
                {
                    // Retrieve a page of messages
                    ExchangeMessagePageInfo pageInfo = client.ListMessagesByPage(folderUri, itemsPerPage, offset);

                    // Process each message in the current page
                    foreach (ExchangeMessageInfo messageInfo in pageInfo.Items)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }

                    // Determine if this was the final page
                    lastPage = pageInfo.LastPage;

                    // Advance the offset for the next iteration
                    offset += itemsPerPage;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
