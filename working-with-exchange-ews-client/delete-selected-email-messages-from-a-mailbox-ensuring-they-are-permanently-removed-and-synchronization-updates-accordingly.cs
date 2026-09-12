using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsDeleteSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange Web Services endpoint and credentials (replace placeholders with real values)
                string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder values
                if (serviceUrl.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password == "password" ||
                    serviceUrl.Contains("YOUR_") ||
                    username.Contains("YOUR_") ||
                    password.Contains("YOUR_"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Basic validation for required parameters
                if (string.IsNullOrWhiteSpace(serviceUrl) || serviceUrl.Contains("YOUR_"))
                {
                    Console.Error.WriteLine("Exchange service URL is not configured.");
                    return;
                }

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // Retrieve mailbox information to get the Inbox folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // List all messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    // Collect URIs of messages that match the deletion criteria
                    List<string> urisToDelete = new List<string>();
                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        if (!string.IsNullOrEmpty(msgInfo.Subject) && msgInfo.Subject.Contains("DeleteMe"))
                        {
                            urisToDelete.Add(msgInfo.UniqueUri);
                        }
                    }

                    if (urisToDelete.Count == 0)
                    {
                        Console.WriteLine("No messages matched the deletion criteria.");
                        return;
                    }

                    // Permanently delete the selected messages
                    client.DeleteItems(urisToDelete.ToArray(), DeletionOptions.DeletePermanently);
                    Console.WriteLine($"{urisToDelete.Count} message(s) permanently deleted from the mailbox.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
