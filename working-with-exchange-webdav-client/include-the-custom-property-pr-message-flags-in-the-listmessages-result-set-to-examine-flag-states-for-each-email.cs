using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using System;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected to avoid external calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create and use the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the message.
                    MailMessage message = client.FetchMessage(info.UniqueUri);

                    // PR_MESSAGE_FLAGS property tag.
                    const string flagHeader = "0x0E07";
                    string flagValue = null;

                    try
                    {
                        flagValue = message.Headers[flagHeader];
                    }
                    catch
                    {
                        // Header not present – ignore.
                    }

                    Console.WriteLine($"Message Subject: {message.Subject}");
                    if (!string.IsNullOrEmpty(flagValue))
                        Console.WriteLine($"PR_MESSAGE_FLAGS: {flagValue}");
                    else
                        Console.WriteLine("PR_MESSAGE_FLAGS not present.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
