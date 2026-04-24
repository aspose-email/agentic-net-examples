using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Gmail client credentials are not set. Skipping execution.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // List all messages in the mailbox.
                    System.Collections.Generic.List<GmailMessageInfo> messageList = gmailClient.ListMessages();

                    // Process each message.
                    foreach (GmailMessageInfo messageInfo in messageList)
                    {
                        // Move the message to the Trash (Gmail's Deleted folder) before permanent removal.
                        // The second parameter 'true' moves the message to Trash.
                        gmailClient.DeleteMessage(messageInfo.Id, true);
                        Console.WriteLine($"Message with ID {messageInfo.Id} moved to Trash.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Gmail operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
