using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace ImapAsyncExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Initialize IMAP client with host, username, password, and security options
                using (ImapClient imapClient = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
                {
                    // Verify connection by sending a NOOP command
                    try
                    {
                        await imapClient.NoopAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP connection failed: {ex.Message}");
                        return;
                    }

                    // Select the INBOX folder
                    await imapClient.SelectFolderAsync("INBOX");

                    // Retrieve up to 10 messages asynchronously
                    ImapMessageInfoCollection messageInfos = await imapClient.ListMessagesAsync(10);

                    // Iterate through the retrieved message infos
                    foreach (ImapMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full message using its unique identifier
                        MailMessage message = await imapClient.FetchMessageAsync(messageInfo.UniqueId);
                        string subject = message.Subject ?? string.Empty;
                        Console.WriteLine($"Subject: {subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}