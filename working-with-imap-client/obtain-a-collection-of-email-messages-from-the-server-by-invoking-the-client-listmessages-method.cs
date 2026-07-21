using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Replace placeholders with actual OAuth access token and email address.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Create Gmail client instance (disposable)
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Retrieve all messages from the mailbox
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                Console.WriteLine($"Total messages: {messages.Count}");
                foreach (GmailMessageInfo info in messages)
                {
                    // Fetch the full message to access Subject and From fields
                    MailMessage mail = gmailClient.FetchMessage(info.Id);
                    Console.WriteLine($"Subject: {mail.Subject}, From: {mail.From}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
