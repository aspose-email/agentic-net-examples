using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services (EWS) connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Ensure the client connection is valid
                try
                {
                    // Retrieve mailbox information (e.g., Inbox URI)
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(mailboxInfo.InboxUri);

                    // Prepare output directory for saved messages
                    string outputDir = "Output";
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Iterate through each message info
                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full mail message using its unique URI
                        MailMessage message = client.FetchMessage(messageInfo.UniqueUri);

                        // Display basic information
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Received: {message.Date}");

                        // Save the message to a local .eml file
                        string safeFileName = $"{Guid.NewGuid()}.eml";
                        string outputPath = Path.Combine(outputDir, safeFileName);
                        try
                        {
                            message.Save(outputPath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Saved to: {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Error saving message: {ioEx.Message}");
                        }

                        // Dispose the fetched message
                        message.Dispose();
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS operation failed: {clientEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
