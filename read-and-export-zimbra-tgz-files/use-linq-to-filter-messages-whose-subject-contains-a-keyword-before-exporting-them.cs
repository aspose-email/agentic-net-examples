using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string serverUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string folderUri = "Inbox";
            string keyword = "Invoice";
            string outputDir = "ExportedMessages";

            // Skip execution when placeholder credentials are detected
            if (serverUrl.Contains("example.com") || username.Contains("@example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to Exchange server
            try
            {
                using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
                {
                    // List messages from the specified folder
                    ExchangeMessageInfoCollection allMessages = client.ListMessages(folderUri);

                    // Filter messages whose Subject contains the keyword (case‑insensitive)
                    var filteredMessages = allMessages
                        .Where(msgInfo => !string.IsNullOrEmpty(msgInfo.Subject) &&
                                          msgInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                    foreach (ExchangeMessageInfo msgInfo in filteredMessages)
                    {
                        // Fetch the full MAPI message
                        using (MapiMessage mapiMessage = client.FetchMapiMessage(msgInfo.UniqueUri))
                        {
                            // Create a safe file name from the subject
                            string subject = string.IsNullOrEmpty(mapiMessage.Subject) ? "Message" : mapiMessage.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                subject = subject.Replace(invalidChar, '_');
                            }

                            string filePath = Path.Combine(outputDir, $"{subject}.msg");

                            // Save the message as .msg
                            try
                            {
                                mapiMessage.Save(filePath);
                                Console.WriteLine($"Exported: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message '{subject}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Connection or operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
