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
            // Placeholder credentials - replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid runtime failures.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Access token is not set. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Retrieve all messages in the mailbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                // Prepare a list to hold IDs of messages that were processed.
                List<string> processedIds = new List<string>();

                foreach (GmailMessageInfo info in messages)
                {
                    try
                    {
                        // Fetch the full message.
                        MailMessage message = gmailClient.FetchMessage(info.Id);

                        // Append the same message back with the "Reviewed" label.
                        // This creates a new copy of the message with the label applied.
                        string newMessageId = gmailClient.AppendMessage(message, "Reviewed");

                        processedIds.Add(newMessageId);
                    }
                    catch (Exception ex)
                    {
                        // Log any errors for individual messages but continue processing.
                        Console.Error.WriteLine($"Failed to process message ID {info.Id}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Processed {processedIds.Count} messages and added the \"Reviewed\" label.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
