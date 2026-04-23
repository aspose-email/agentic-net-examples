using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials - replace with real values.
            string gmailAccessToken = "YOUR_ACCESS_TOKEN";
            string gmailDefaultEmail = "user@example.com";

            string imapHost = "imap.example.com";
            int imapPort = 993;
            string imapUsername = "user@example.com";
            string imapPassword = "YOUR_PASSWORD";

            // Guard against placeholder credentials.
            if (gmailAccessToken.StartsWith("YOUR_") ||
                imapHost.Contains("example.com") ||
                imapUsername.StartsWith("user@") && imapPassword.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(gmailAccessToken, gmailDefaultEmail);

            // Initialize IMAP client.
            using (ImapClient imapClient = new ImapClient(imapHost, imapPort, imapUsername, imapPassword))
            {
                // Connect and authenticate.
                try
                {
                    imapClient.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP connection/authentication failed: {ex.Message}");
                    return;
                }

                // Retrieve Gmail messages.
                List<GmailMessageInfo> gmailMessages;
                try
                {
                    gmailMessages = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                // Retrieve IMAP messages (only IDs for comparison).
                List<string> imapMessageIds;
                try
                {
                    imapMessageIds = new List<string>();
                    foreach (ImapMessageInfo info in imapClient.ListMessages())
                    {
                        imapMessageIds.Add(info.UniqueId);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list IMAP messages: {ex.Message}");
                    return;
                }

                // Simple comparison: count of messages.
                Console.WriteLine($"Gmail messages count: {gmailMessages.Count}");
                Console.WriteLine($"IMAP messages count: {imapMessageIds.Count}");

                // Compare subjects of first few messages if available.
                int compareCount = Math.Min(5, Math.Min(gmailMessages.Count, imapMessageIds.Count));
                for (int i = 0; i < compareCount; i++)
                {
                    try
                    {
                        MailMessage gmailMsg = gmailClient.FetchMessage(gmailMessages[i].Id);
                        MailMessage imapMsg = imapClient.FetchMessage(imapMessageIds[i]);

                        string gmailSubject = gmailMsg.Subject ?? string.Empty;
                        string imapSubject = imapMsg.Subject ?? string.Empty;

                        Console.WriteLine($"Message {i + 1}:");
                        Console.WriteLine($"  Gmail Subject: {gmailSubject}");
                        Console.WriteLine($"  IMAP  Subject: {imapSubject}");
                        Console.WriteLine($"  Subjects match: {gmailSubject.Equals(imapSubject, StringComparison.OrdinalIgnoreCase)}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error comparing message {i + 1}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
