using System;
using System.Collections.Generic;
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
            // Placeholder credentials – skip execution in CI environments
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping IMAP operation due to placeholder credentials.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    await client.SelectFolderAsync("INBOX");

                    // Retrieve all messages in the folder
                    IList<ImapMessageInfo> allMessages = await client.ListMessagesAsync();

                    // Define the cutoff date (messages older than one year)
                    DateTime cutoffDate = DateTime.Now.AddYears(-1);

                    // Collect messages that need the \Deleted flag
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();

                    foreach (ImapMessageInfo msgInfo in allMessages)
                    {
                        // Fetch the full message to inspect its internal date
                        MailMessage mail = await client.FetchMessageAsync(msgInfo.UniqueId);
                        if (mail.Date < cutoffDate)
                        {
                            messagesToDelete.Add(msgInfo);
                        }
                    }

                    // Set the \Deleted flag on the selected messages
                    if (messagesToDelete.Count > 0)
                    {
                        await client.AddMessageFlagsAsync(messagesToDelete, ImapMessageFlags.Deleted);
                    }

                    // Expunge deleted messages by unselecting the folder without the doNotExpunge flag
                    await client.UnselectFolderAsync(false);
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
