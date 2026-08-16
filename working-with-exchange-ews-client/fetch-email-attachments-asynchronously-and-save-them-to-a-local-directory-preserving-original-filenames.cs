using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // ----- Configuration -----
            string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string outputDirectory = "Attachments";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // ----- Create EWS client -----
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve mailbox information to get the Inbox URI.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // List messages with attachment information.
                    ExchangeMessageInfoCollection messages = client.ListMessages(
                        inboxUri,
                        ExchangeListMessagesOptions.FetchAttachmentInformation);

                    // Cast to async interface if supported.
                    if (client is IAsyncEwsClient asyncClient)
                    {
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            // Each message may contain multiple attachments.
                            foreach (dynamic attachmentInfo in messageInfo.Attachments)
                            {
                                try
                                {
                                    // Fetch the attachment asynchronously.
                                    Attachment attachment = await asyncClient.FetchAttachmentAsync(
                                        (string)attachmentInfo.Uri,
                                        CancellationToken.None);

                                    // Preserve the original attachment filename.
                                    string safeFileName = Path.GetFileName((string)attachmentInfo.Name ?? attachment.Name);
                                    string filePath = Path.Combine(outputDirectory, safeFileName);

                                    // Save the attachment to disk.
                                    try
                                    {
                                        attachment.Save(filePath);
                                        Console.WriteLine($"Saved attachment: {filePath}");
                                    }
                                    catch (Exception saveEx)
                                    {
                                        Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {saveEx.Message}");
                                    }
                                }
                                catch (Exception fetchEx)
                                {
                                    Console.Error.WriteLine($"Failed to fetch attachment '{attachmentInfo.Name}': {fetchEx.Message}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("The EWS client does not support asynchronous operations.");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
