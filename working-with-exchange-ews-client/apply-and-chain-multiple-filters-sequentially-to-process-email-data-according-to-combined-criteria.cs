using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // Client connection safety guard
                try
                {
                    // Initialize the EWS client (replace placeholders with real values)
                    using (IEWSClient client = EWSClient.GetEWSClient(
                        "https://mail.example.com/EWS/Exchange.asmx",
                        new NetworkCredential("username", "password")))
                    {
                        // Build a composite query:
                        //   - Unread messages (no IsRead flag)
                        //   - Subject contains "Report"
                        //   - From address contains "example.com"
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                        builder.Subject.Contains("Report");
                        builder.From.Contains("example.com");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages from the Inbox that match the query
                        string inboxUri = client.MailboxInfo.InboxUri;
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query, false);

                        // Process each message
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            // Fetch the full message using its unique URI
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                            }
                        }
                    }
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"Connection error: {connEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
