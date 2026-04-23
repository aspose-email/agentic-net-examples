using Aspose.Email;
using System;
using System.Threading;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;

                    // Retrieve messages from INBOX
                    ImapMessageInfoCollection messages = client.ListMessagesAsync(
                        folderName: "INBOX",
                        modificationSequence: 0,
                        retrieveRecursively: false,
                        messageExtraFields: null,
                        connection: null,
                        token: CancellationToken.None).Result;

                    // Iterate and log SequenceNumber and UniqueId
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"SequenceNumber: {info.SequenceNumber}, UniqueId: {info.UniqueId}");
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
