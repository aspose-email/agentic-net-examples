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
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution if placeholder credentials are detected.
            if (string.IsNullOrWhiteSpace(clientId) ||
                clientId.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                clientSecret.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                refreshToken.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(defaultEmail) ||
                defaultEmail.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Gmail client credentials are not set. Skipping label update.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            if (gmailClient == null)
            {
                Console.Error.WriteLine("Gmail client initialization returned null.");
                return;
            }

            using (gmailClient as IDisposable)
            {
                List<GmailMessageInfo> messages;
                try
                {
                    // Retrieve all messages in the mailbox.
                    messages = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                foreach (GmailMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Fetch the full message.
                        MailMessage mailMessage = gmailClient.FetchMessage(messageInfo.Id);

                        // Append the "Reviewed" label to the message.
                        gmailClient.AppendMessage(mailMessage, "Reviewed");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add label to message ID {messageInfo.Id}: {ex.Message}");
                        // Continue processing remaining messages.
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
