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

            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.Contains("example.com"))
            {
                Console.Error.WriteLine("Gmail credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                List<GmailMessageInfo> allMessages;
                try
                {
                    allMessages = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                // Determine cutoff date (30 days ago).
                DateTime cutoff = DateTime.UtcNow.AddDays(-30);
                var idsToDelete = new List<string>();

                foreach (GmailMessageInfo info in allMessages)
                {
                    // GmailMessageInfo does not expose a Date property; use InternalDate instead.
                    // If InternalDate is unavailable, skip the message.
                    DateTime? internalDate = null;
                    try
                    {
                        // Aspose.Email defines InternalDate as nullable DateTime.
                        internalDate = (DateTime?)info.GetType().GetProperty("InternalDate")?.GetValue(info);
                    }
                    catch { }

                    if (internalDate.HasValue && internalDate.Value < cutoff)
                    {
                        idsToDelete.Add(info.Id);
                    }
                }

                if (idsToDelete.Count == 0)
                {
                    Console.WriteLine("No messages older than 30 days were found.");
                    return;
                }

                // Batch delete – Gmail API does not provide a single batch delete method in IGmailClient,
                // so we delete each message individually.
                foreach (string id in idsToDelete)
                {
                    try
                    {
                        // Permanently delete the message (moveToTrash = false).
                        gmailClient.DeleteMessage(id, false);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete message {id}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Deleted {idsToDelete.Count} Gmail messages older than 30 days.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
