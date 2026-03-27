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
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Create Gmail client using the OAuth credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Retrieve a list of messages
                System.Collections.Generic.List<GmailMessageInfo> messages = gmailClient.ListMessages();

                // Iterate through messages and print their subjects
                foreach (GmailMessageInfo messageInfo in messages)
                {
                    // Fetch the full message to access its subject
                    MailMessage fullMessage = gmailClient.FetchMessage(messageInfo.Id);
                    Console.WriteLine("Subject: " + fullMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
