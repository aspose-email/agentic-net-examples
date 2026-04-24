using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            ProcessAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static async Task ProcessAsync()
    {
        // Placeholder credentials detection
        const string host = "imap.example.com";
        const string username = "username";
        const string password = "password";

        if (host.Contains("example.com"))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
            return;
        }

        // Create and connect the IMAP client
        using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
        {
            try
            {
                const int itemsPerPage = 50;
                int pageOffset = 0;
                PageSettings settings = new PageSettings
                {
                    FolderName = "INBOX",
                    AscendingSorting = true
                };

                while (true)
                {
                    // Retrieve a page of message infos asynchronously
                    ImapPageInfo pageInfo = await client.ListMessagesByPageAsync(itemsPerPage, pageOffset, settings);

                    foreach (ImapMessageInfo msgInfo in pageInfo.Items)
                    {
                        // Fetch the full message asynchronously
                        using (MailMessage message = await client.FetchMessageAsync(msgInfo.UniqueId))
                        {
                            // Example processing: output the subject
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }

                    if (pageInfo.LastPage)
                        break;

                    pageOffset++;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
            }
        }
    }
}
