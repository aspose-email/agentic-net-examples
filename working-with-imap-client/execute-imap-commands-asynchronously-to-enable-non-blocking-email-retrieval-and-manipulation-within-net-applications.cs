using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapAsyncExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Placeholder credentials – replace with real values to run against an IMAP server
            string host = "your_imap_host";
            int port = 993;
            string username = "your_username";
            string password = "your_password";

            // Guard against executing network calls with placeholder data
            if (host.StartsWith("your_") || username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            try
            {
                // Create and configure the IMAP client (auto‑connects on first operation)
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Asynchronously retrieve the list of messages in the INBOX folder
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync("INBOX");
                    Console.WriteLine($"Total messages in INBOX: {messageInfos.Count}");

                    // Display subjects of the first few messages without fetching full content
                    int messagesToShow = Math.Min(5, messageInfos.Count);
                    for (int i = 0; i < messagesToShow; i++)
                    {
                        ImapMessageInfo info = messageInfos[i];
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
            }
        }
    }
}
