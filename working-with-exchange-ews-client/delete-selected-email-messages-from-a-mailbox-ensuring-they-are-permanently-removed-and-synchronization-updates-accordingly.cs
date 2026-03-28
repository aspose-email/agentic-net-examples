using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with placeholder credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // List all messages in the mailbox
            List<GmailMessageInfo> allMessages = gmailClient.ListMessages();

            // Select messages to delete (e.g., first 5 messages)
            int messagesToDelete = Math.Min(5, allMessages.Count);
            for (int i = 0; i < messagesToDelete; i++)
            {
                GmailMessageInfo messageInfo = allMessages[i];
                // Permanently delete the message (moveToTrash = false)
                gmailClient.DeleteMessage(messageInfo.Id, false);
                Console.WriteLine($"Deleted message with ID: {messageInfo.Id}");
            }

            // Optional: synchronize changes (Gmail client updates automatically)
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
