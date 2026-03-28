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
            // Initialize Gmail client with OAuth 2.0 credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            using (gmailClient)
            {
                // Retrieve the list of messages
                var messages = gmailClient.ListMessages(); // List<GmailMessageInfo>

                foreach (var info in messages)
                {
                    // Fetch the full message to access its properties
                    MailMessage fullMessage = gmailClient.FetchMessage(info.Id);
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
