using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapAsyncExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Initialize the IMAP client with host, port, credentials and security options.
                using (ImapClient client = new ImapClient("imap.example.com", 993, "user@example.com", "password", SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Asynchronously select the INBOX folder. This operation implicitly establishes the connection.
                        await client.SelectFolderAsync("INBOX");
                        Console.WriteLine("Asynchronous connection established and INBOX selected.");
                    }
                    catch (Exception ex)
                    {
                        // Handle connection or folder selection errors.
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}