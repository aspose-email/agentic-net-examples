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
            // Placeholder credentials and server information.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder values.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Parameters for the attachment to retrieve.
            string targetFolder = "Inbox";
            string attachmentFileName = "document.pdf";
            string savePath = Path.Combine(Environment.CurrentDirectory, "Attachments", attachmentFileName);

            // Ensure the target directory exists.
            try
            {
                string targetDirectory = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare directory: {dirEx.Message}");
                return;
            }

            // Connect to Exchange server.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    client.PreAuthenticate = true;

                    // List messages with attachment information.
                    ExchangeListMessagesOptions listOptions = ExchangeListMessagesOptions.FetchAttachmentInformation;
                    ExchangeMessageInfoCollection messages = client.ListMessages(targetFolder, listOptions);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        if (messageInfo.HasAttachments)
                        {
                            foreach (ExchangeAttachmentInfo attachmentInfo in messageInfo.Attachments)
                            {
                                if (string.Equals(attachmentInfo.Name, attachmentFileName, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Fetch the attachment.
                                    Attachment attachment = client.FetchAttachment(attachmentInfo.AttachmentUri);

                                    // Save the attachment to disk.
                                    try
                                    {
                                        attachment.Save(savePath);
                                        Console.WriteLine($"Attachment saved to: {savePath}");
                                    }
                                    catch (Exception saveEx)
                                    {
                                        Console.Error.WriteLine($"Failed to save attachment: {saveEx.Message}");
                                    }

                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Specified attachment not found in the folder.");
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
