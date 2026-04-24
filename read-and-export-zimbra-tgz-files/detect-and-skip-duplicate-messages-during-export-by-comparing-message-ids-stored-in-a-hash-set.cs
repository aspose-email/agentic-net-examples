using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if they are not real.
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Ensure the export directory exists.
            string exportFolder = "ExportedMessages";
            try
            {
                if (!Directory.Exists(exportFolder))
                {
                    Directory.CreateDirectory(exportFolder);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare export folder: {dirEx.Message}");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                try
                {
                    // List all messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    if (messages == null)
                    {
                        Console.Error.WriteLine("No messages retrieved.");
                        return;
                    }

                    // HashSet to track processed Message-IDs.
                    HashSet<string> processedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Guard against null MessageId.
                        string messageId = messageInfo.MessageId;
                        if (string.IsNullOrEmpty(messageId))
                        {
                            continue;
                        }

                        // Skip duplicate messages.
                        if (processedIds.Contains(messageId))
                        {
                            continue;
                        }

                        processedIds.Add(messageId);

                        // Fetch the full message.
                        MailMessage mailMessage;
                        try
                        {
                            mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageId}: {fetchEx.Message}");
                            continue;
                        }

                        // Build a safe file name.
                        string safeSubject = string.IsNullOrEmpty(mailMessage.Subject) ? "NoSubject" : mailMessage.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string filePath = Path.Combine(exportFolder, $"{safeSubject}_{Guid.NewGuid():N}.eml");

                        // Save the message to disk.
                        try
                        {
                            mailMessage.Save(filePath, SaveOptions.DefaultEml);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message {messageId} to file: {saveEx.Message}");
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
