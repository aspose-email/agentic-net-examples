using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid live network calls in CI.
            if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("your") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.Contains("your") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.Contains("your"))
            {
                Console.Error.WriteLine("Gmail credentials are not set. Skipping execution.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // List all messages in the mailbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                foreach (GmailMessageInfo info in messages)
                {
                    // Fetch the full message to inspect attachments.
                    using (MailMessage message = gmailClient.FetchMessage(info.Id))
                    {
                        foreach (Attachment attachment in message.Attachments)
                        {
                            // Check attachment size (>5 MB).
                            if (attachment.ContentStream != null && attachment.ContentStream.Length > 5 * 1024 * 1024)
                            {
                                Console.WriteLine($"Message ID with large attachment: {info.Id}");
                                // No need to check other attachments for this message.
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
