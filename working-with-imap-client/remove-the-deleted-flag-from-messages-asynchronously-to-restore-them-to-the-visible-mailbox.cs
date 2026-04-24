using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected.
            if (host.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            try
            {
                // Create and use the IMAP client.
                using (ImapClient client = new ImapClient(host, port, username, password))
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the selected folder.
                    IList<ImapMessageInfo> messages = await client.ListMessagesAsync();

                    if (messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Collect the unique identifiers of the messages.
                    List<string> uids = new List<string>();
                    foreach (ImapMessageInfo info in messages)
                    {
                        uids.Add(info.UniqueId);
                    }

                    // Remove the Deleted flag from all retrieved messages.
                    await client.RemoveMessageFlagsAsync(uids, ImapMessageFlags.Deleted);

                    Console.WriteLine("Deleted flag removed from messages.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
