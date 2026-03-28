using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace ImapAsyncExample
{
    class Program
    {
        // Entry point – async to allow awaiting Aspose.Email async methods.
        static async Task Main(string[] args)
        {
            try
            {
                // Wrap the client in a using block to ensure proper disposal.
                using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder.
                        await client.SelectFolderAsync("INBOX");

                        // Asynchronously retrieve information about all messages in the selected folder.
                        ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                        if (messageInfos == null || messageInfos.Count == 0)
                        {
                            Console.WriteLine("No messages found in the INBOX.");
                            return;
                        }

                        // Take a few message UIDs to demonstrate fetching.
                        List<string> uids = messageInfos
                            .Take(5)                     // limit to first 5 messages
                            .Select(info => info.UniqueId)
                            .ToList();

                        // Asynchronously fetch the full messages using the UIDs.
                        IEnumerable<MailMessage> messages = await client.FetchMessagesAsync(uids);

                        // Output basic details of each fetched message.
                        foreach (MailMessage msg in messages)
                        {
                            Console.WriteLine($"Subject: {msg.Subject}");
                            Console.WriteLine($"From: {msg.From}");
                            Console.WriteLine($"Date: {msg.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during IMAP operations.
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur while creating or disposing the client.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
