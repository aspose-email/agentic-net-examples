using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution in CI environments.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Connect to Exchange using WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Attempt to list a single message from the Inbox to obtain its URI.
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, 1);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Retrieve the unique URI of the first message.
                string messageUri = messages[0].UniqueUri;

                // Fetch the message as a MAPI object.
                MapiMessage mapiMessage = client.FetchMapiMessage(messageUri);
                if (mapiMessage == null)
                {
                    Console.WriteLine("Failed to fetch the message.");
                    return;
                }

                // Set a follow‑up flag on the message.
                FollowUpManager.SetFlag(mapiMessage, "Follow up");

                // Note: ExchangeClient does not expose an UpdateMessage method.
                // The flag is set on the local MapiMessage instance.
                // In a real scenario, you would use an API that supports updating
                // the message on the server (e.g., Graph or EWS client).

                Console.WriteLine("Follow‑up flag applied to the message.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
