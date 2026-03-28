using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with placeholder credentials
                IGmailClient gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");

                using (gmailClient)
                {
                    try
                    {
                        // List all messages in the mailbox
                        List<GmailMessageInfo> messages = gmailClient.ListMessages();

                        foreach (GmailMessageInfo messageInfo in messages)
                        {
                            // Fetch the full message using its Id
                            using (MailMessage mailMessage = gmailClient.FetchMessage(messageInfo.Id))
                            {
                                // Example processing: output subject to console
                                Console.WriteLine("Subject: " + mailMessage.Subject);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during Gmail operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}
