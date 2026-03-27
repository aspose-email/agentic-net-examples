using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace CloudConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with dummy credentials
                IGmailClient gmailClient = GmailClient.GetInstance(
                    clientId: "clientId",
                    clientSecret: "clientSecret",
                    refreshToken: "refreshToken",
                    defaultEmail: "user@example.com");

                // Ensure the client is disposed properly
                using (gmailClient)
                {
                    // List messages in the mailbox
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messages)
                    {
                        // Fetch the full message to access the subject
                        MailMessage fullMessage = gmailClient.FetchMessage(info.Id);
                        Console.WriteLine(fullMessage.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
