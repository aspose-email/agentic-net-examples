using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values when running in production
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected (prevents CI failures)
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client using the factory method (no direct constructor)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Get the URI of the Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List all messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        try
                        {
                            // Archive the message into the default Archive folder
                            client.ArchiveItem(info.UniqueUri, "Archive");
                            Console.WriteLine($"Archived message: {info.Subject}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to archive message '{info.Subject}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while processing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
