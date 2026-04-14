using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailPaginationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Page size for each request
                int pageSize = 50;
                int offset = 0;
                bool morePages = true;

                // Create EWS client inside a using block to ensure disposal
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Loop through pages until the last page is reached
                        while (morePages)
                        {
                            // Retrieve a page of messages from the Inbox folder
                            ExchangeMessagePageInfo pageInfo = client.ListMessagesByPage("Inbox", pageSize, offset);

                            // Process each message in the current page
                            foreach (ExchangeMessageInfo messageInfo in pageInfo.Items)
                            {
                                Console.WriteLine($"Subject: {messageInfo.Subject}");
                            }

                            // Determine if there are more pages
                            morePages = !pageInfo.LastPage;

                            // Update offset for the next page
                            offset = pageInfo.PageOffset + pageInfo.ItemsPerPage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
