using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSearchFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values)
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials
                if (username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                    return;
                }

                // Build a composite query (acts as a SearchFilterCollection)
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.From.Contains("alice@example.com");
                queryBuilder.Subject.Contains("Invoice");
                MailQuery compositeQuery = queryBuilder.GetQuery();

                // Connect to EWS
                IEWSClient ewsClient = null;
                try
                {
                    ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password);
                    // Query the Inbox folder with the composite query
                    string folderName = "Inbox";
                    ExchangeMessageInfoCollection messages = ewsClient.ListMessages(folderName, compositeQuery);

                    // Output results
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
                finally
                {
                    // Dispose the client if it implements IDisposable
                    if (ewsClient is IDisposable disposableClient)
                    {
                        disposableClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
