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
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string targetLabel = "AttachmentsLabel";

            // Simple guard to avoid real network calls with placeholder data.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR_ACCESS_TOKEN"))
            {
                Console.Error.WriteLine("Gmail credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Retrieve all messages in the mailbox.
                    List<GmailMessageInfo> messagesInfo = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message to inspect attachments.
                        MailMessage fullMessage = gmailClient.FetchMessage(info.Id);

                        // Check if the message contains any attachments.
                        if (fullMessage.Attachments.Count > 0)
                        {
                            // Append the message to the designated label (folder).
                            // The overload AppendMessage(MailMessage, string) applies the label.
                            string newMessageId = gmailClient.AppendMessage(fullMessage, targetLabel);

                            // Delete the original message to complete the move.
                            gmailClient.DeleteMessage(info.Id);
                            
                            Console.WriteLine($"Message {info.Id} moved to label '{targetLabel}' (new ID: {newMessageId}).");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
