using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping IMAP operations.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Define maximum operation duration (30 seconds)
                client.Timeout = 30000;

                try
                {
                    // Perform a lightweight operation to verify the connection
                    ImapFolderInfoCollection folders = client.ListFolders();
                    Console.WriteLine("Folders retrieved:");
                    foreach (ImapFolderInfo folder in folders)
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
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
