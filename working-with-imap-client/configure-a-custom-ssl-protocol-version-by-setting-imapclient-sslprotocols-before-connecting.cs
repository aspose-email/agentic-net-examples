using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Base;
using Aspose.Email.Clients;
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

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping IMAP connection.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Configure a specific SSL/TLS protocol version (e.g., TLS 1.2)
                client.SupportedEncryption = EncryptionProtocols.Tls12;

                try
                {
                    // Trigger connection with a lightweight async operation (synchronously waited)
                    ImapFolderInfo inboxInfo = client.GetFolderInfoAsync(ImapFolderInfo.InBox).GetAwaiter().GetResult();
                    Console.WriteLine($"Connected. INBOX contains {inboxInfo.TotalMessageCount} messages.");
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
