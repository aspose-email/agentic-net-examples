using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;


public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details – replace with real values for actual use.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server interaction.");
                return;
            }

            // Create and use the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Get the Inbox folder URI.
                    string inboxFolder = client.MailboxInfo.InboxUri;

                    // Retrieve all messages from the Inbox.
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxFolder);

                    // Define the case‑sensitive filter criteria.
                    string filterSubject = "Test Subject";

                    // Iterate through the messages and apply the case‑sensitive filter.
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        if (String.Equals(messageInfo.Subject, filterSubject, StringComparison.Ordinal))
                        {
                            Console.WriteLine("Subject: " + messageInfo.Subject);
                            Console.WriteLine("From   : " + (messageInfo.From != null ? messageInfo.From.ToString() : "N/A"));
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Operation error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled error: " + ex.Message);
        }
    }
}
