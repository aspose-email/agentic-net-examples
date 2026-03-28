using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GmailOAuthSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace these placeholder values with real credentials.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Create the Gmail client using OAuth 2.0 credentials.
                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                try
                {
                    // List messages in the mailbox.
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();

                    Console.WriteLine($"Total messages: {messages.Count}");

                    foreach (GmailMessageInfo info in messages)
                    {
                        // Fetch the full message to access detailed properties.
                        using (MailMessage fullMessage = gmailClient.FetchMessage(info.Id))
                        {
                            string subject = fullMessage.Subject ?? string.Empty;
                            string from = fullMessage.From?.Address ?? string.Empty;
                            Console.WriteLine($"Subject: {subject}");
                            Console.WriteLine($"From: {from}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                    return;
                }
                finally
                {
                    // Ensure the client is properly disposed.
                    if (gmailClient != null)
                    {
                        gmailClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
