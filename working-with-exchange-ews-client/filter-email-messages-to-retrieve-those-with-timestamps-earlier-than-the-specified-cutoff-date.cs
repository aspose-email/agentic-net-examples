using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – in a real scenario replace with actual server URL and credentials.
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client inside a using block to ensure proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Define the cutoff date.
                DateTime cutoffDate = new DateTime(2023, 1, 1);

                // Retrieve the inbox folder URI.
                string inboxUri = client.MailboxInfo.InboxUri;

                // List all messages in the inbox.
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Iterate through the messages and filter by InternalDate.
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Use InternalDate for comparison as per validation rules.
                    DateTime internalDate = info.InternalDate;

                    if (internalDate < cutoffDate)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"Internal Date: {internalDate}");
                        Console.WriteLine($"URI: {info.UniqueUri}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
