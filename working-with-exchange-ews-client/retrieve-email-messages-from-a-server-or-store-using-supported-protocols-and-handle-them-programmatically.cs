using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

// Author: Aspose.Email example – retrieves messages from Exchange via EWS
public class Program
{
    public static void Main(string[] args)
    {
        // Connection parameters (replace with real values)
        string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Create the EWS client and ensure it is disposed properly
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Obtain mailbox information to get the Inbox URI
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);

                foreach (ExchangeMessageInfo msgInfo in messageInfos)
                {
                    // Fetch the full message
                    MailMessage message = client.FetchMessage(msgInfo.UniqueUri);

                    // Display basic details
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"Received: {msgInfo.InternalDate}");

                    // Save the message to a file (guarded I/O)
                    string outputDir = Path.Combine("SavedMessages");
                    string outputPath = Path.Combine(outputDir, $"{msgInfo.UniqueUri.GetHashCode()}.eml");
                    try
                    {
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                        message.Save(outputPath);
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    }

                    // Dispose the fetched MailMessage
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
            return;
        }
    }
}
