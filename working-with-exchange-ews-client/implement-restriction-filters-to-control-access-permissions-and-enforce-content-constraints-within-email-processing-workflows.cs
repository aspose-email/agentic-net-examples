using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values for demonstration purposes
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip actual server connection
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping actual server connection.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create and use the EWS client within a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Retrieve messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Iterate through each message and apply restriction filters
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Use InternalDate for the message's internal timestamp (avoid using Date property)
                    DateTime internalDate = messageInfo.InternalDate;

                    // Example filter: only process messages older than 30 days
                    if (internalDate < DateTime.UtcNow.AddDays(-30))
                    {
                        // Output subject and internal date for filtered messages
                        Console.WriteLine("Subject: " + messageInfo.Subject);
                        Console.WriteLine("Internal Date (UTC): " + internalDate.ToString("u"));
                        Console.WriteLine();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
