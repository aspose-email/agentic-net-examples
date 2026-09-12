using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace MailboxArchivalSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service connection parameters
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Get mailbox information to obtain the Inbox URI
                    ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // List all messages in the Inbox (non‑recursive) without a query
                    ExchangeMessageInfoCollection messages = ewsClient.ListMessages(inboxUri, null);

                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        try
                        {
                            // Fetch the full MailMessage (preserves all metadata)
                            MailMessage mailMsg = ewsClient.FetchMessage(msgInfo.UniqueUri);

                            // Convert MailMessage to MapiMessage for archival
                            MapiMessage mapMsg = MapiMessage.FromMailMessage(mailMsg);

                            // Archive the message – this moves it to the user's archive mailbox
                            ewsClient.ArchiveItem(inboxUri, mapMsg);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to archive message {msgInfo.UniqueUri}: {ex.Message}");
                            // Continue with next message
                        }
                    }

                    Console.WriteLine("Archival process completed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
