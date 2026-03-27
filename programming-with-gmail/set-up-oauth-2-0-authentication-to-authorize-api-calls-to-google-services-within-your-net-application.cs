using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth 2.0 credentials
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            IGmailClient gmailClient = null;
            try
            {
                // Create Gmail client instance
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                // List messages (IDs only)
                var messages = gmailClient.ListMessages();
                foreach (GmailMessageInfo messageInfo in messages)
                {
                    MailMessage fullMessage = null;
                    try
                    {
                        // Fetch full message to access subject
                        fullMessage = gmailClient.FetchMessage(messageInfo.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {messageInfo.Id}: {ex.Message}");
                        continue;
                    }

                    using (fullMessage)
                    {
                        Console.WriteLine($"Subject: {fullMessage.Subject}");
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
