using Aspose.Email.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapDeleteExample
{
    class Program
    {
        // Entry point of the console application.
        static async Task Main(string[] args)
        {
            // Top‑level exception guard.
            try
            {
                // Placeholder connection parameters.
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";
                string messageUid = "12345"; // Unique identifier of the message to delete.

                // Guard against executing real network calls with placeholder credentials.
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP delete operation.");
                    return;
                }

                // Create and connect the IMAP client inside a using block to ensure disposal.
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Wrap client operations in a try/catch to surface friendly errors.
                    try
                    {
                        // Permanently delete the message identified by its UID.
                        // The second argument (true) commits the deletion immediately.
                        await client.DeleteMessageAsync(messageUid, true, CancellationToken.None);

                        Console.WriteLine($"Message with UID '{messageUid}' has been permanently deleted.");
                    }
                    catch (Exception ex)
                    {
                        // Log any errors that occur during the delete operation.
                        Console.Error.WriteLine($"Error deleting message: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors that escape the inner blocks.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
