using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and server URL
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder values to avoid real network calls
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Access mailbox information to verify the session
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                        Console.WriteLine($"Connected to mailbox. Inbox URI: {mailboxInfo.InboxUri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error accessing mailbox info: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect EWS client: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
