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
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Enable logging to monitor latency (optional)
                imapClient.EnableLogger = true;
                imapClient.LogFileName = "imap_log.txt";

                // Select the INBOX folder asynchronously
                await imapClient.SelectFolderAsync("INBOX");

                // Retrieve a limited set of messages asynchronously (e.g., first 10)
                ImapMessageInfoCollection messageInfos = await imapClient.ListMessagesAsync(10);

                Console.WriteLine($"Fetched {messageInfos.Count} message(s) from INBOX.");

                if (messageInfos.Count > 0)
                {
                    // Fetch the first message's full content asynchronously
                    ImapMessageInfo firstInfo = messageInfos[0];
                    MailMessage firstMessage = await imapClient.FetchMessageAsync(firstInfo.UniqueId);
                    Console.WriteLine($"Subject of first message: {firstMessage.Subject}");
                }

                // Disable logging after operations
                imapClient.EnableLogger = false;
                imapClient.ResetLogSettings();
            }
        }
        catch (ImapException imapEx)
        {
            Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}