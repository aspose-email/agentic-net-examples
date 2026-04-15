using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values if needed.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping server connection.");
                return;
            }

            // Connect to the IMAP server inside a using block to ensure disposal.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.ValidateCredentials();

                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve folder information.
                    ImapFolderInfo inboxInfo = client.GetFolderInfo("INBOX");

                    // TotalMessageCount gives the number of messages in the folder.
                    int totalMessages = inboxInfo.TotalMessageCount;

                    Console.WriteLine($"Total messages in INBOX: {totalMessages}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
