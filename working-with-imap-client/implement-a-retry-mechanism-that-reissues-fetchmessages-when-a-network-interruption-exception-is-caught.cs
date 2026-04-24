using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Skipping IMAP operations due to placeholder credentials.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                // Select the INBOX folder
                client.SelectFolder("INBOX");

                // Retrieve a collection of message infos
                ImapMessageInfoCollection infos = client.ListMessages();

                // Take the first 5 message UIDs (or fewer if not enough messages)
                List<string> uids = infos.Select(info => info.UniqueId).Take(5).ToList();

                if (uids.Count == 0)
                {
                    Console.WriteLine("No messages found in the INBOX.");
                    return;
                }

                // Retry logic for fetching messages
                const int maxRetries = 3;
                int attempt = 0;
                IList<MailMessage> fetchedMessages = null;

                while (attempt < maxRetries)
                {
                    try
                    {
                        // Fetch messages by UID
                        fetchedMessages = client.FetchMessages(uids);
                        break; // Success, exit retry loop
                    }
                    catch (ImapException ex) when (IsNetworkError(ex))
                    {
                        attempt++;
                        if (attempt == maxRetries)
                        {
                            Console.Error.WriteLine($"Failed to fetch messages after {maxRetries} attempts: {ex.Message}");
                            return;
                        }
                        Console.WriteLine($"Network error encountered. Retrying {attempt}/{maxRetries}...");
                        Thread.Sleep(2000); // Simple back‑off delay
                    }
                }

                // Process fetched messages (e.g., display subjects)
                foreach (MailMessage message in fetchedMessages)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple heuristic to decide if an ImapException is network‑related
    private static bool IsNetworkError(ImapException ex)
    {
        string msg = ex.Message?.ToLowerInvariant() ?? string.Empty;
        return msg.Contains("network") || msg.Contains("connection") || msg.Contains("timeout");
    }
}
