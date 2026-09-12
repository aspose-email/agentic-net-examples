using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;

namespace SubjectFilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Example demonstrates how to filter Exchange EWS messages by subject.
                string host = "ews.example.com";
                string username = "user@example.com";
                string password = "password";
                string folder = "Inbox";
                string subjectKeyword = "Invoice";

                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Preserve the client variable name as required.
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    // Build a MailQuery that filters messages containing the subject keyword.
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains(subjectKeyword);
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages matching the query from the specified folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(folder, query);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}, From: {info.From}, Date: {info.InternalDate}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
