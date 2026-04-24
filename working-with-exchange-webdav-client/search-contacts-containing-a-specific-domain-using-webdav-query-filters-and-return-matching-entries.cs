using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls in CI
            if (exchangeUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                // Build a WebDAV query to find contacts whose email address contains the target domain
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.From.Contains("@contoso.com");
                MailQuery query = builder.GetQuery();

                // Use the Inbox folder as a fallback for contacts (adjust as needed)
                string contactsFolderUri = client.MailboxInfo.InboxUri;

                // Retrieve messages (contacts) matching the query
                ExchangeMessageInfoCollection messages = client.ListMessages(contactsFolderUri, query.ToString());

                foreach (ExchangeMessageInfo messageInfo in messages)
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
