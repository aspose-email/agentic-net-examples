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
            // Placeholder credentials – replace with real values or skip execution.
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder values to avoid real network calls in CI.
            if (serverUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(serverUrl, new NetworkCredential(username, password)))
            {
                // Retrieve mailbox information.
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Calendar URI: {mailboxInfo.CalendarUri}");
                Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");

                // Update client preferences.
                client.TimezoneId = "Pacific Standard Time";
                client.UseSlashAsFolderSeparator = true;
                Console.WriteLine("Updated client timezone and folder separator settings.");

                // List all mailboxes in the organization.
                try
                {
                    var mailboxes = client.ListMailboxes();
                    Console.WriteLine("Mailboxes in the organization:");
                    foreach (var mb in mailboxes)
                    {
                        Console.WriteLine($"- {mb}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list mailboxes: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
