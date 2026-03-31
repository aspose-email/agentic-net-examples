using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client safely
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client as IDisposable)
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Traverse each message info and retrieve properties
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.InternalDate}");
                    Console.WriteLine($"Unique URI: {info.UniqueUri}");
                    Console.WriteLine($"Has Attachments: {info.HasAttachments}");
                    Console.WriteLine();

                    // Optionally fetch the full message for further processing
                    try
                    {
                        using (MailMessage fullMessage = client.FetchMessage(info.UniqueUri))
                        {
                            // Example: output body preview
                            Console.WriteLine($"Body Preview: {fullMessage.Body?.Substring(0, Math.Min(100, fullMessage.Body?.Length ?? 0))}");
                            Console.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message '{info.UniqueUri}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
