using Aspose.Email.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Placeholder credentials guard – avoid real network calls in CI
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operation.");
                return;
            }

            // Client connection safety guard
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the folder to process (INBOX by default)
                    client.SelectFolder("INBOX");

                    const int itemsPerPage = 100;   // Number of messages per page
                    int pageOffset = 0;             // Offset for the current page

                    while (true)
                    {
                        // Asynchronously retrieve a page of message infos
                        ImapPageInfo pageInfo = await client.ListMessagesByPageAsync(
                            itemsPerPage,
                            pageOffset,
                            new PageSettings());

                        // Process each message in the current page
                        foreach (ImapMessageInfo msgInfo in pageInfo.Items)
                        {
                            // Fetch the full message asynchronously
                            MailMessage message = await client.FetchMessageAsync(msgInfo.UniqueId);
                            Console.WriteLine($"Subject: {message.Subject}");
                        }

                        // If this is the last page, exit the loop
                        if (pageInfo.LastPage)
                            break;

                        // Move to the next page
                        pageOffset += itemsPerPage;
                    }
                }
                catch (Exception ex)
                {
                    // Friendly error output for client operations
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Global exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
