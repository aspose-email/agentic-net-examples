using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
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
                        // Retrieve list of messages in the mailbox
                        List<GmailMessageInfo> messageList = gmailClient.ListMessages();

                        if (messageList != null && messageList.Count > 0)
                        {
                            // Choose the first message to delete (replace with desired criteria)
                            string messageId = messageList[0].Id;

                            // Delete the selected message permanently
                            gmailClient.DeleteMessage(messageId);

                            Console.WriteLine($"Deleted message with Id: {messageId}");
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the mailbox.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during Gmail operations
                        Console.Error.WriteLine($"Gmail operation error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
