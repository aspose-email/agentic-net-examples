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
            // Initialize the Gmail client with dummy credentials.
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // Ensure the client is disposed after use.
            using (gmailClient)
            {
                // Retrieve the list of messages.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();
                Console.WriteLine($"Total messages: {messages.Count}");

                // Iterate through messages and fetch full details.
                foreach (GmailMessageInfo info in messages)
                {
                    using (MailMessage fullMessage = gmailClient.FetchMessage(info.Id))
                    {
                        Console.WriteLine($"Subject: {fullMessage.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
