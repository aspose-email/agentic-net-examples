using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace EmailRestrictionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder Exchange server details.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip external calls when placeholders are detected.
            bool placeholdersDetected = mailboxUri.Contains("example.com") ||
                                        username.Contains("example.com") ||
                                        password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (placeholdersDetected)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            try
            {
                // Initialize the Exchange client.
                IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);

                // Build a query that matches all messages.
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                MailQuery query = queryBuilder.GetQuery();

                // List up to 10 messages from the Inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", 10, query);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch each message using its unique URI.
                    MailMessage message = client.FetchMessage(info.UniqueUri);

                    // Output basic information to the console.
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    // Use InternalDate instead of Date/SentDate as per validation rules.
                    Console.WriteLine($"Date: {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                // Log any errors without throwing.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
