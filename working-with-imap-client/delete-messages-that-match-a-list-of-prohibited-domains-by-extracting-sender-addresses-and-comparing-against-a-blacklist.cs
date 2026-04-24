using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // List of prohibited sender domains
            List<string> prohibitedDomains = new List<string>
            {
                "spam.com",
                "baddomain.org"
            };

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    IList<ImapMessageInfo> allMessages = client.ListMessages();

                    // Collect messages that match prohibited domains
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();

                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        // Fetch the full message to examine the sender
                        MailMessage fullMessage = client.FetchMessage(messageInfo.UniqueId);
                        if (fullMessage != null && fullMessage.From != null && fullMessage.From.Address != null)
                        {
                            string senderDomain = GetDomainFromEmail(fullMessage.From.Address);
                            foreach (string prohibitedDomain in prohibitedDomains)
                            {
                                if (string.Equals(senderDomain, prohibitedDomain, StringComparison.OrdinalIgnoreCase))
                                {
                                    messagesToDelete.Add(messageInfo);
                                    break;
                                }
                            }
                        }
                    }

                    // Delete the identified messages and commit the deletions
                    if (messagesToDelete.Count > 0)
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} message(s) deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the prohibited domains.");
                    }
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

    // Helper method to extract domain part from an email address
    private static string GetDomainFromEmail(string email)
    {
        int atIndex = email.LastIndexOf('@');
        if (atIndex >= 0 && atIndex < email.Length - 1)
        {
            return email.Substring(atIndex + 1);
        }
        return string.Empty;
    }
}
