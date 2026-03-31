using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection details – replace with real values.
                string serverUrl = "https://exchange.example.com/ews/exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (serverUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Initialize the WebDAV Exchange client.
                using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
                {
                    // The mailbox of the user whose messages we want to access.
                    string otherUserMailbox = "otheruser@example.com";

                    // Retrieve mailbox information for the target user.
                    ExchangeMailboxInfo otherMailboxInfo;
                    try
                    {
                        otherMailboxInfo = client.GetMailboxInfo(otherUserMailbox);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to obtain mailbox info for '{otherUserMailbox}': {ex.Message}");
                        return;
                    }

                    // List messages in the target user's Inbox.
                    try
                    {
                        var inboxMessages = client.ListMessages(otherMailboxInfo.InboxUri);
                        foreach (var msgInfo in inboxMessages)
                        {
                            // Fetch each message to read its subject (or other properties).
                            MailMessage message = client.FetchMessage(msgInfo.UniqueUri);
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
