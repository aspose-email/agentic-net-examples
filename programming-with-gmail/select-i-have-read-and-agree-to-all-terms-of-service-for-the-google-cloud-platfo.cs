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
            // Initialize Gmail client with dummy OAuth token and default email
            IGmailClient gmailClient = GmailClient.GetInstance("accessToken", "user@example.com");
            using (gmailClient)
            {
                // List messages in the mailbox
                var messages = gmailClient.ListMessages();
                foreach (var messageInfo in messages)
                {
                    // Fetch the full message to access its subject
                    MailMessage fullMessage = gmailClient.FetchMessage(messageInfo.Id);
                    Console.WriteLine(fullMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
