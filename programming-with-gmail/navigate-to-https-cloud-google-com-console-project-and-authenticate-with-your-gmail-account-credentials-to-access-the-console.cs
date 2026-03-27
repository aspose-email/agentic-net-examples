using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // OAuth credentials (replace with real values)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Create Gmail client instance
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Retrieve list of messages (metadata only)
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                // Iterate through messages and fetch full details to read the subject
                foreach (GmailMessageInfo messageInfo in messages)
                {
                    // Fetch the complete message using its Id
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
