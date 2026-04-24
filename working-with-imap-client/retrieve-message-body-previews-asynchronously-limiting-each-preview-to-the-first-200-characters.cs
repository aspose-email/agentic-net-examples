using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message by its unique ID
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                        // Get a preview of the body (first 200 characters)
                        string body = message.Body ?? string.Empty;
                        string preview = body.Length > 200 ? body.Substring(0, 200) : body;

                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"Preview: {preview}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
