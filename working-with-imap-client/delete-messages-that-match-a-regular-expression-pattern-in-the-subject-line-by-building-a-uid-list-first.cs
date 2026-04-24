using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

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

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example") || username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Regular expression pattern to match in the subject line
            string subjectPattern = @"^Spam.*";

            // Create and connect the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    imapClient.SelectFolder("INBOX");

                    // Retrieve all messages in the selected folder
                    ImapMessageInfoCollection allMessages = imapClient.ListMessages();

                    // Build a list of messages whose subject matches the regex pattern
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    foreach (ImapMessageInfo msgInfo in allMessages)
                    {
                        string subject = msgInfo.Subject ?? string.Empty;
                        if (Regex.IsMatch(subject, subjectPattern, RegexOptions.IgnoreCase))
                        {
                            messagesToDelete.Add(msgInfo);
                        }
                    }

                    // Delete the matched messages and commit the deletions immediately
                    if (messagesToDelete.Count > 0)
                    {
                        imapClient.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} message(s) deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the specified subject pattern.");
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
}
