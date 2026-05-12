using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.AntiSpam;

namespace SpamFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configuration (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";
                string spamFolderName = "Spam";
                double spamThreshold = 0.7; // 0.0 – 1.0

                // Guard against placeholder credentials to avoid real network calls during CI
                if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password"))
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                    return;
                }

                // Connect to the IMAP server
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Ensure the spam folder exists
                        if (!client.ExistFolder(spamFolderName))
                        {
                            client.CreateFolder(spamFolderName);
                        }

                        // Retrieve all messages in the current folder
                        ImapMessageInfoCollection messageInfos = client.ListMessages();

                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            // Fetch the full message for analysis
                            using (MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueId))
                            {
                                // Analyze spam probability
                                SpamAnalyzer analyzer = new SpamAnalyzer();
                                double spamScore = analyzer.Test(mailMessage);

                                // If the score exceeds the threshold, move the message to the spam folder
                                if (spamScore > spamThreshold)
                                {
                                    // MoveMessage(destinationFolder, messageUri)
                                    client.MoveMessage(spamFolderName, messageInfo.UniqueId);
                                    Console.WriteLine($"Message '{mailMessage.Subject}' moved to '{spamFolderName}' (Spam score: {spamScore:F2}).");
                                }
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
                return;
            }
        }
    }
}
