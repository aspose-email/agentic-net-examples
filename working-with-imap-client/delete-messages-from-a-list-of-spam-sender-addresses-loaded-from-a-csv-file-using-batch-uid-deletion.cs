using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration (replace with real values or keep placeholders)
            string imapHost = "imap.example.com";
            int imapPort = 993;
            string imapUsername = "user@example.com";
            string imapPassword = "password";
            string spamCsvPath = "spam_senders.csv";

            // Guard against placeholder credentials/host
            if (imapHost.Contains("example.com") || imapUsername.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Verify CSV file existence
            if (!File.Exists(spamCsvPath))
            {
                Console.Error.WriteLine($"CSV file not found: {spamCsvPath}");
                return;
            }

            // Load spam sender addresses from CSV (one address per line)
            List<string> spamSenders = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(spamCsvPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string trimmed = line.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            spamSenders.Add(trimmed);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read CSV file: {ex.Message}");
                return;
            }

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(imapHost, imapPort, imapUsername, imapPassword, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Prepare a list to hold messages that match spam senders
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();

                    foreach (ImapMessageInfo info in allMessages)
                    {
                        // Fetch minimal headers to get the sender address
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        if (message.From != null && message.From.Count > 0)
                        {
                            string fromAddress = message.From[0].Address;
                            // Case‑insensitive comparison with spam list
                            bool isSpam = spamSenders.Any(s => string.Equals(s, fromAddress, StringComparison.OrdinalIgnoreCase));
                            if (isSpam)
                            {
                                messagesToDelete.Add(info);
                            }
                        }
                    }

                    if (messagesToDelete.Count == 0)
                    {
                        Console.WriteLine("No spam messages found.");
                        return;
                    }

                    // Delete messages in batch and commit immediately
                    client.DeleteMessages(messagesToDelete, true);
                    Console.WriteLine($"{messagesToDelete.Count} spam message(s) deleted.");
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
