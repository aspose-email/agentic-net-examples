using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace DeleteGmailMessageSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize Gmail client with dummy credentials
                using (IGmailClient gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com"))
                {
                    try
                    {
                        // Retrieve the list of messages in the mailbox
                        List<GmailMessageInfo> messages = gmailClient.ListMessages();

                        if (messages != null && messages.Count > 0)
                        {
                            // Pick the first message to delete
                            GmailMessageInfo firstMessage = messages[0];
                            string messageId = firstMessage.Id;

                            // Delete the selected message
                            gmailClient.DeleteMessage(messageId);

                            Console.WriteLine($"Deleted message with Id: {messageId}");
                        }
                        else
                        {
                            Console.WriteLine("No messages found to delete.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during message operations
                        Console.Error.WriteLine($"Operation error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur during client initialization
                Console.Error.WriteLine($"Initialization error: {ex.Message}");
            }
        }
    }
}
