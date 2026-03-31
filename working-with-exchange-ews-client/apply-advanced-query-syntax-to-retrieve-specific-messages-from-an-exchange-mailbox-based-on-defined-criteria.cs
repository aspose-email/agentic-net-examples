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
        try
        {
            // Placeholder connection details – replace with real values when running against a live server.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard: avoid making real network calls when placeholders are used.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange service URL detected. Skipping execution.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                try
                {
                    // Build an Advanced Query Syntax (AQS) query:
                    //   - Messages received on or after 1 Jan 2023 (InternalDate >= startDate)
                    //   - Subject contains the word "Report"
                    DateTime startDate = new DateTime(2023, 1, 1);
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.InternalDate.Since(startDate);
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that match the query.
                    ExchangeMessageInfoCollection messages = client.ListMessages(
                        client.MailboxInfo.InboxUri,
                        query);

                    // Output basic information for each matching message.
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"Internal Date: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message retrieval: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
