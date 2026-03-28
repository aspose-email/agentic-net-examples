using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Initialize Gmail client (connection safety)
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",          // OAuth client ID
                "clientSecret",      // OAuth client secret
                "refreshToken",      // OAuth refresh token
                "user@example.com")) // Gmail address
            {
                try
                {
                    // Retrieve all messages in the mailbox
                    List<GmailMessageInfo> messages = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messages)
                    {
                        // Fetch the full message to examine its properties
                        MailMessage fullMessage = gmailClient.FetchMessage(info.Id);

                        // Example criterion: delete messages whose subject contains "Unwanted"
                        if (fullMessage?.Subject != null && fullMessage.Subject.Contains("Unwanted"))
                        {
                            // Permanently delete the message
                            gmailClient.DeleteMessage(info.Id);
                            Console.WriteLine($"Deleted message Id: {info.Id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors that occur during message processing
                    Console.Error.WriteLine($"Message processing error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors (e.g., client initialization failures)
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
