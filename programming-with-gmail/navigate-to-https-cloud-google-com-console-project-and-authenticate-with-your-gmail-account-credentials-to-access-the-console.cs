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
            // Initialize Gmail client with OAuth credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance("clientId", "clientSecret", "refreshToken", "user@example.com"))
            {
                try
                {
                    // List messages in the mailbox
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();
                    Console.WriteLine($"Total messages: {messages.Count}");
                    foreach (GmailMessageInfo info in messages)
                    {
                        Console.WriteLine($"Message Id: {info.Id}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
        }
    }
}
