using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Replace these placeholder values with actual credentials obtained from Google Developer Console
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            // Create the Gmail client using the provided credentials
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);

            // Verify the client works by listing messages in the mailbox
            List<GmailMessageInfo> messages = gmailClient.ListMessages();
            Console.WriteLine($"Retrieved {messages.Count} messages from the Gmail account.");

            // If there is at least one message, fetch its full content and display the subject
            if (messages.Count > 0)
            {
                GmailMessageInfo firstInfo = messages[0];
                MailMessage fullMessage = gmailClient.FetchMessage(firstInfo.Id);
                Console.WriteLine($"First message subject: {fullMessage.Subject}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
