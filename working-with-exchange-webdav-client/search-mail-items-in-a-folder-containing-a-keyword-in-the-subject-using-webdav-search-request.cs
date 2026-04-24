using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder Exchange server details
            string exchangeUrl = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are detected
            if (exchangeUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange URL detected. Skipping network call.");
                return;
            }

            // Create Exchange client (WebDAV)
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                try
                {
                    // Keyword to search in the subject
                    string keyword = "Invoice";

                    // Build the search query using ExchangeQueryBuilder
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains(keyword);
                    MailQuery query = builder.GetQuery();

                    // Folder to search (Inbox)
                    string folderUri = client.MailboxInfo.InboxUri;

                    // Execute the search
                    ExchangeMessageInfoCollection messages = client.ListMessages(folderUri, query.ToString());

                    // Output subjects of matching messages
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during search: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
