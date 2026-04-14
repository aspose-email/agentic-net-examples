using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Get the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List all messages in the Inbox
                    ExchangeMessageInfoCollection inboxMessages = client.ListMessages(inboxUri);
                    if (inboxMessages == null || inboxMessages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                        return;
                    }

                    // Collect the URIs of messages to be processed
                    List<string> messageUris = new List<string>();
                    foreach (ExchangeMessageInfo msgInfo in inboxMessages)
                    {
                        // Use UniqueUri as required by EWS APIs
                        if (!string.IsNullOrEmpty(msgInfo.UniqueUri))
                        {
                            messageUris.Add(msgInfo.UniqueUri);
                        }
                    }

                    if (messageUris.Count == 0)
                    {
                        Console.WriteLine("No valid message URIs found.");
                        return;
                    }

                    // Mark messages as junk and move them to the Junk folder
                    // Parameters: isJunk = true, moveItem = true, messageUriEn = messageUris
                    string[] movedMessageIds = client.MarkAsJunk(true, true, messageUris);
                    Console.WriteLine($"Moved {movedMessageIds.Length} messages to the Junk folder.");

                    // Mark each moved message as read
                    foreach (string movedId in movedMessageIds)
                    {
                        client.SetReadFlag(movedId, true);
                    }
                    Console.WriteLine("All moved messages have been marked as read.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
