using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip live network calls when placeholders are detected
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Ensure the attachments directory exists
            string attachmentsDir = "Attachments";
            try
            {
                if (!Directory.Exists(attachmentsDir))
                {
                    Directory.CreateDirectory(attachmentsDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachments directory: {ex.Message}");
                return;
            }

            // Create Gmail client
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

            // Retrieve list of messages (unread filtering not directly supported in this API)
            List<GmailMessageInfo> messageInfos = null;
            try
            {
                messageInfos = gmailClient.ListMessages();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                return;
            }

            foreach (GmailMessageInfo info in messageInfos)
            {
                // Fetch the full message
                MailMessage message = null;
                try
                {
                    message = gmailClient.FetchMessage(info.Id);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message {info.Id}: {ex.Message}");
                    continue;
                }

                using (message)
                {
                    // Save each attachment to the local directory
                    foreach (Attachment attachment in message.Attachments)
                    {
                        string filePath = Path.Combine(attachmentsDir, attachment.Name);
                        try
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.ContentStream.CopyTo(fileStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {ex.Message}");
                        }
                    }

                    // Mark the message as read – not directly available via IGmailClient; placeholder for future implementation
                    // Example: gmailClient.ModifyMessageLabels(info.Id, removeLabel: "UNREAD");
                }
            }

            // Dispose the Gmail client if it implements IDisposable
            if (gmailClient is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
