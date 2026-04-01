using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSharedMailboxSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder values – replace with real server URL and credentials.
                string ewsUrl = "https://example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";
                string sharedMailboxEmail = "shared@example.com";

                // Guard against executing with placeholder credentials.
                if (ewsUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                    return;
                }

                // Create the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
                {
                    // Obtain information about the shared mailbox.
                    ExchangeMailboxInfo sharedMailboxInfo;
                    try
                    {
                        sharedMailboxInfo = client.GetMailboxInfo(sharedMailboxEmail);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to retrieve shared mailbox info: {ex.Message}");
                        return;
                    }

                    // List messages in the shared mailbox's Inbox folder.
                    try
                    {
                        ExchangeMessageInfoCollection messageInfos = client.ListMessages(sharedMailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo messageInfo in messageInfos)
                        {
                            // Fetch the full message using its unique URI.
                            using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while listing or fetching messages: {ex.Message}");
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
