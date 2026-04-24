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
            // Placeholder connection settings
            string exchangeUri = "https://example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Ensure output directory exists
            string outputPath = "MessageSummary.csv";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Connect to Exchange server
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Attempt to access the Inbox to validate connectivity
                    string inboxUri;
                    try
                    {
                        inboxUri = client.MailboxInfo.InboxUri;
                    }
                    catch (Exception inboxEx)
                    {
                        Console.Error.WriteLine($"Failed to retrieve Inbox URI: {inboxEx.Message}");
                        return;
                    }

                    // List messages in the Inbox
                    ExchangeMessageInfoCollection messages;
                    try
                    {
                        messages = client.ListMessages(inboxUri);
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                        return;
                    }

                    // Write CSV header and message details
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            writer.WriteLine("Subject,ReceivedDate");
                            foreach (ExchangeMessageInfo messageInfo in messages)
                            {
                                string subject = messageInfo.Subject?.Replace("\"", "\"\"") ?? string.Empty;
                                string date = messageInfo.InternalDate.ToString("o"); // ISO 8601 format
                                writer.WriteLine($"\"{subject}\",{date}");
                            }
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ioEx.Message}");
                        return;
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
