using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth 2.0 credentials (replace with real values)
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            // Create Gmail client instance safely
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

            // Ensure the client is disposed properly
            using (gmailClient)
            {
                try
                {
                    // List messages in the mailbox
                    var messages = gmailClient.ListMessages();
                    foreach (GmailMessageInfo messageInfo in messages)
                    {
                        // Fetch the full message to access its properties
                        MailMessage fullMessage = gmailClient.FetchMessage(messageInfo.Id);
                        Console.WriteLine($"Subject: {fullMessage.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Gmail operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
