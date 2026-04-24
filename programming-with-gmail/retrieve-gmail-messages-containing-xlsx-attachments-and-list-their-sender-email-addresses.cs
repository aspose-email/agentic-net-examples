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
            string defaultEmail = "your-email@gmail.com";

            // Skip execution if placeholder credentials are detected.
            if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("your-") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.Contains("your-") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.Contains("your-") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.Contains("your-"))
            {
                Console.Error.WriteLine("Gmail credentials are not set. Skipping Gmail access.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Retrieve all messages in the mailbox.
                List<GmailMessageInfo> messagesInfo = gmailClient.ListMessages();

                foreach (GmailMessageInfo info in messagesInfo)
                {
                    // Fetch the full message to inspect attachments.
                    using (MailMessage message = gmailClient.FetchMessage(info.Id))
                    {
                        // Check each attachment for .xlsx extension.
                        foreach (Attachment attachment in message.Attachments)
                        {
                            if (attachment.Name != null && attachment.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                            {
                                // Output the sender's email address.
                                if (message.From != null)
                                {
                                    Console.WriteLine(message.From.Address);
                                }
                                break; // No need to check further attachments for this message.
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
