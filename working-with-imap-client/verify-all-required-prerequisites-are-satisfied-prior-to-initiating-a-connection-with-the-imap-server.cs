using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapPrerequisiteCheck
{
    // Author: Aspose.Email example - verifies IMAP connection prerequisites before connecting.
    class Program
    {
        static void Main()
        {
            // IMAP server connection parameters (replace with real values).
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Simple placeholder detection – skip network call if parameters look like defaults.
            bool isPlaceholder = string.IsNullOrWhiteSpace(host) ||
                                 string.IsNullOrWhiteSpace(username) ||
                                 string.IsNullOrWhiteSpace(password) ||
                                 host.Contains("example", StringComparison.OrdinalIgnoreCase) ||
                                 username.Contains("example", StringComparison.OrdinalIgnoreCase) ||
                                 password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Attempt to create the IMAP client and perform a lightweight operation.
            try
            {
                // SecurityOptions lives in Aspose.Email.Clients namespace.
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // List top‑level folders to verify the connection works.
                    ImapFolderInfoCollection folders = imapClient.ListFolders();
                    Console.WriteLine($"Successfully connected to IMAP server '{host}'.");
                    Console.WriteLine($"Found {folders.Count} top‑level folder(s):");
                    foreach (ImapFolderInfo folder in folders)
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to IMAP server: {ex.Message}");
            }
        }
    }
}
