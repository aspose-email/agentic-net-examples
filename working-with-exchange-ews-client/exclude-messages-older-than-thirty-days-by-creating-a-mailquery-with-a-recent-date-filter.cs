using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters – replace with actual values.
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client inside a using block to ensure proper disposal.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Build a MailQuery that selects messages received within the last 30 days.
                        DateTime recentDate = DateTime.Now.AddDays(-30);
                        MailQueryBuilder queryBuilder = new MailQueryBuilder();
                        queryBuilder.InternalDate.Since(recentDate);
                        MailQuery recentQuery = queryBuilder.GetQuery();

                        // List messages from the Inbox folder that match the query.
                        // The third parameter (true) enables recursive search if needed.
                        ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", recentQuery);

                        // Output the subject of each retrieved message.
                        foreach (var msgInfo in messages)
                        {
                            Console.WriteLine("Subject: " + msgInfo.Subject);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during query execution or message retrieval.
                        Console.Error.WriteLine("Error while retrieving messages: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur during client creation/connection.
                Console.Error.WriteLine("Failed to connect to Exchange server: " + ex.Message);
                return;
            }
        }
    }
}
