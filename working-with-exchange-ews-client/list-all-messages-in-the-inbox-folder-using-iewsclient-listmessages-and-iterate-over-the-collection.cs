using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against using placeholder credentials.
            if (mailboxUri.Contains("example.com") ||
                string.Equals(username, "username", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(password, "password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Skipping EWS operation because placeholder credentials are detected.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List all messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages();

                    // Iterate over the collection and display basic information.
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Subject may be null for some items; handle gracefully.
                        string subject = messageInfo.Subject ?? "<No Subject>";
                        Console.WriteLine($"Subject: {subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
