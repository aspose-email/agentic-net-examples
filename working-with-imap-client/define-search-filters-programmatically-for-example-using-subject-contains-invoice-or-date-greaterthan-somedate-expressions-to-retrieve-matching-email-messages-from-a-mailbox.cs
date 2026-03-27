using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSearchExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define EWS service URL and credentials
                string serviceUrl = "https://example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create and dispose the EWS client safely
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    try
                    {
                        // Build the search query: Subject contains "Invoice" and SentDate within the last 30 days
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Invoice");
                        DateTime fromDate = DateTime.UtcNow.AddDays(-30);
                        builder.SentDate.Greater(fromDate);
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages from the Inbox that match the query
                        ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri, query);

                        // Iterate over the results and display basic information
                        foreach (ExchangeMessageInfo info in messageInfos)
                        {
                            // Fetch the full message to access its properties
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Sent: {message.Date}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}