using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

namespace AsposeEmailPaginationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder values to avoid runtime network errors.
                if (string.IsNullOrEmpty(host) || host.Contains("example.com") ||
                    string.IsNullOrEmpty(username) || username.Contains("example.com") ||
                    string.IsNullOrEmpty(password))
                {
                    Console.Error.WriteLine("IMAP connection parameters are placeholders. Skipping execution.");
                    return;
                }

                // Use a using block to ensure the client is disposed properly.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials by selecting the INBOX folder.
                        client.SelectFolder("INBOX");

                        const int itemsPerPage = 100; // Adjust page size as needed.
                        int pageOffset = 0;

                        while (true)
                        {
                            // Retrieve a page of messages.
                            ImapPageInfo pageInfo = client.ListMessagesByPageAsync(itemsPerPage, pageOffset, new PageSettings()).GetAwaiter().GetResult();

                            // Process each message in the current page.
                            foreach (ImapMessageInfo messageInfo in pageInfo.Items)
                            {
                                // Example: output basic information without loading full message content.
                                Console.WriteLine($"UID: {messageInfo.UniqueId}, Subject: {messageInfo.Subject}, From: {messageInfo.From}");
                            }

                            // Exit loop if this is the last page.
                            if (pageInfo.LastPage)
                            {
                                break;
                            }

                            pageOffset++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during IMAP operations: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
