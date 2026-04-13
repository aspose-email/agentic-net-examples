using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox connection parameters (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client inside a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build an AQS query to find items with size greater than 5 MB (5 * 1024 * 1024 bytes)
                ExchangeAdvancedSyntaxQueryBuilder queryBuilder = new ExchangeAdvancedSyntaxQueryBuilder();
                MailQuery sizeQuery = queryBuilder.Size.Greater(5 * 1024 * 1024);

                // List messages in the Inbox folder that match the query (non‑recursive)
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", sizeQuery);

                // Output basic information about each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"Size (bytes): {info.Size}");
                    Console.WriteLine($"Has Attachments: {info.HasAttachments}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error stream without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
