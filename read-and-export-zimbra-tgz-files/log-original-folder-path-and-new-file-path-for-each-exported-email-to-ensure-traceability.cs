using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are detected
            if (serverUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Output directory for exported EML files
            string outputDir = Path.Combine(Environment.CurrentDirectory, "ExportedEmails");

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Initialize Exchange client inside a using block for proper disposal
            try
            {
                using (ExchangeClient client = new ExchangeClient(serverUri, new NetworkCredential(username, password)))
                {
                    // Folder to export messages from (e.g., Inbox)
                    string folderUri = "Inbox";

                    // Retrieve the collection of message infos
                    ExchangeMessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages(folderUri);
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                        return;
                    }

                    // Iterate over each message and export it
                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        // Build a unique file name for the exported email
                        string newFilePath = Path.Combine(outputDir, $"{Guid.NewGuid()}.eml");

                        // Save the message to the local file system
                        try
                        {
                            client.SaveMessage(messageInfo.UniqueUri, newFilePath);
                            Console.WriteLine($"Exported from folder '{folderUri}' to file '{newFilePath}'.");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message '{messageInfo.UniqueUri}': {saveEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
