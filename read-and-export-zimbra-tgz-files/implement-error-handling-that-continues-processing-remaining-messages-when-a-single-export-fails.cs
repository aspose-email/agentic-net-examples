using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip execution to avoid network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Prepare output directory.
            string outputDirectory = "ExportedMessages";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder.
                string folderUri = "Inbox";
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages(folderUri);
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                    return;
                }

                // Process each message individually.
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Build a safe file name.
                    string safeSubject = messageInfo.Subject ?? "NoSubject";
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(invalidChar, '_');
                    }
                    string fileName = $"{safeSubject}.eml";
                    string filePath = Path.Combine(outputDirectory, fileName);

                    // Export the message, handling errors per item.
                    try
                    {
                        client.SaveMessage(messageInfo.UniqueUri, filePath);
                        Console.WriteLine($"Exported: {filePath}");
                    }
                    catch (Exception exportEx)
                    {
                        Console.Error.WriteLine($"Failed to export message '{messageInfo.Subject}': {exportEx.Message}");
                        // Continue with the next message.
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
