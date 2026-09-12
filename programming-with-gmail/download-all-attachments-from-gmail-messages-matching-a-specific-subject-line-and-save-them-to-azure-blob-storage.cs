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
            // Placeholder credentials – replace with real values when available.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string subjectFilter = "Target Subject";
            string outputFolder = "Attachments";

            // Ensure the output directory exists before any file operations.
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create the Gmail client. No explicit connection method is required.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            // Retrieve the list of messages in the mailbox.
            List<Aspose.Email.Clients.Google.GmailMessageInfo> messages = gmailClient.ListMessages();

            foreach (Aspose.Email.Clients.Google.GmailMessageInfo messageInfo in messages)
            {
                MailMessage mailMessage = null;
                try
                {
                    // Fetch the full message using its unique identifier.
                    mailMessage = gmailClient.FetchMessage(messageInfo.Id);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message {messageInfo.Id}: {ex.Message}");
                    continue;
                }

                if (mailMessage == null)
                {
                    continue;
                }

                // Process only messages whose subject contains the specified filter text.
                if (!string.IsNullOrEmpty(mailMessage.Subject) && mailMessage.Subject.Contains(subjectFilter))
                {
                    foreach (Attachment attachment in mailMessage.Attachments)
                    {
                        string attachmentPath = Path.Combine(outputFolder, attachment.Name);
                        try
                        {
                            // Save each attachment to the local folder (acting as a placeholder for Azure Blob storage).
                            using (FileStream fileStream = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                            {
                                using (Stream attachmentStream = attachment.ContentStream)
                                {
                                    attachmentStream.CopyTo(fileStream);
                                }
                            }
                            Console.WriteLine($"Saved attachment: {attachmentPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment {attachment.Name}: {ex.Message}");
                        }
                    }
                }

                // Dispose the MailMessage to release resources.
                mailMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
