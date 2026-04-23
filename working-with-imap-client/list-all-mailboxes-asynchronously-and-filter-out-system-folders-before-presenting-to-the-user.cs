using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                // List all folders asynchronously.
                ImapFolderInfoCollection allFolders = await client.ListFoldersAsync();

                // Known system folder names (case‑insensitive).
                HashSet<string> systemFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "INBOX",
                    "Sent",
                    "Sent Items",
                    "Drafts",
                    "Trash",
                    "Deleted Items",
                    "Junk",
                    "Junk Mail",
                    "Flagged",
                    "Important",
                    "All",
                    "All Mail",
                    "Archive",
                    "Archives"
                };

                Console.WriteLine("User folders:");
                foreach (var folder in allFolders)
                {
                    if (!systemFolders.Contains(folder.Name))
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
