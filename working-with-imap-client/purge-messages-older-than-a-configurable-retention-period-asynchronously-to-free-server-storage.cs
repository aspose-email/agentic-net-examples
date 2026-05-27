using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Configuration
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";
            int retentionDays = 30; // Purge messages older than this many days

            // Guard against placeholder credentials/host
            if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder server/credentials detected. Skipping purge operation.");
                return;
            }

            // Calculate cutoff date (UTC)
            DateTime cutoffDateUtc = DateTime.UtcNow.AddDays(-retentionDays);

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Validate credentials
                    await client.ValidateCredentialsAsync();

                    // Select the target folder
                    await client.SelectFolderAsync(folderName);

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync();

                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message to inspect its Date header
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                        // Compare the message date (converted to UTC) with the cutoff
                        DateTime messageDateUtc = message.Date.ToUniversalTime();
                        if (messageDateUtc < cutoffDateUtc)
                        {
                            // Delete the message and commit the deletion immediately
                            await client.DeleteMessageAsync(info.UniqueId, true);
                            Console.WriteLine($"Deleted message UID {info.UniqueId} dated {messageDateUtc:u}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
