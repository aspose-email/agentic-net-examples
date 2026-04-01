using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string userEmail = "user@example.com";

                // Guard against placeholder credentials to avoid unwanted network calls.
                if (clientId == "clientId" ||
                    clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping network operation.");
                    return;
                }

                // Create the Gmail client using the factory method.
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
                {
                    // Retrieve all messages. The Gmail API supports query strings like "is:unread",
                    // but IGmailClient.ListMessages() does not expose a query overload.
                    // Therefore, fetch all messages and process them.
                    List<GmailMessageInfo> allMessages = gmailClient.ListMessages();

                    foreach (GmailMessageInfo msgInfo in allMessages)
                    {
                        // Fetch the full message content.
                        using (MailMessage message = gmailClient.FetchMessage(msgInfo.Id))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard – write error to standard error.
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
