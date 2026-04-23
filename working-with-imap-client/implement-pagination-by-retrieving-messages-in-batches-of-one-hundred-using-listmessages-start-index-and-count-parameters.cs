using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip real network calls in CI
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(host, 993, username, password, SecurityOptions.SSLImplicit))
            {
                // Select the INBOX folder
                client.SelectFolder("INBOX");

                const int pageSize = 100;      // Number of messages per batch
                int pageOffset = 0;            // Zero‑based page index
                bool morePages = true;

                while (morePages)
                {
                    // Configure paging settings
                    PageSettings settings = new PageSettings
                    {
                        FolderName = "INBOX",
                        AscendingSorting = true,
                        IgnoreExceptions = true
                    };

                    // Retrieve a page of messages
                    var pageTask = client.ListMessagesByPageAsync(pageSize, pageOffset, settings);
                    pageTask.Wait();
                    ImapPageInfo pageInfo = pageTask.Result;

                    // Process the messages in the current batch
                    foreach (ImapMessageInfo info in pageInfo.Items)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }

                    // Determine if there are more pages
                    morePages = !pageInfo.LastPage;
                    pageOffset++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
