using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with placeholder credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // List messages in the mailbox
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                // Extract message IDs into a string array
                string[] messageIds = messages.Select(m => m.Id).ToArray();

                // Output each message ID
                foreach (string id in messageIds)
                {
                    Console.WriteLine(id);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
