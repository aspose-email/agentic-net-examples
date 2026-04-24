using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "imap.example.com";
            string username = "user@example.com";
            string accessToken = "ya29.a0AfH6SM..."; // OAuth2 access token

            // Guard against placeholder values to avoid unwanted network calls.
            if (host.Contains("example.com") || string.IsNullOrWhiteSpace(accessToken))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Create the IMAP client using OAuth2 token (useOAuth = true).
            using (ImapClient client = new ImapClient(host, username, accessToken, true))
            {
                // Validate the credentials asynchronously.
                bool isValid = await client.ValidateCredentialsAsync();
                if (!isValid)
                {
                    Console.Error.WriteLine("IMAP authentication failed.");
                    return;
                }

                Console.WriteLine("IMAP connection established successfully.");

                // Example: fetch the list of messages from the INBOX folder.
                ImapMessageInfoCollection messages = await client.ListMessagesAsync("INBOX", false, CancellationToken.None);
                Console.WriteLine($"Number of messages in INBOX: {messages.Count}");

                // Optionally, fetch the first message and display its subject.
                if (messages.Count > 0)
                {
                    MailMessage firstMessage = await client.FetchMessageAsync(messages[0].UniqueId);
                    Console.WriteLine($"First message subject: {firstMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
