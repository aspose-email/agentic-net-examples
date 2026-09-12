using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace ExchangeMailboxSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection parameters – replace with real values.
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client.
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Get mailbox information.
                        ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                        string inboxUri = mailboxInfo.InboxUri;

                        // List messages in the Inbox.
                        ExchangeMessageInfoCollection messageInfos = ewsClient.ListMessages(inboxUri);

                        // Ensure output directory exists.
                        string outputDir = "OutputMessages";

                        // Skip external calls when placeholder credentials are used
                        if (username.Contains("example.com") || password == "password")
                        {
                            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                            return;
                        }

                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        foreach (ExchangeMessageInfo messageInfo in messageInfos)
                        {
                            try
                            {
                                // Fetch the full message.
                                MailMessage mailMessage = ewsClient.FetchMessage(messageInfo.UniqueUri);

                                // Build a safe file name from the message URI.
                                string safeFileName = messageInfo.UniqueUri.Replace("/", "_").Replace("\\", "_") + ".eml";
                                string filePath = Path.Combine(outputDir, safeFileName);

                                // Save the message to disk.
                                mailMessage.Save(filePath);
                                Console.WriteLine($"Saved message to: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to process message {messageInfo.UniqueUri}: {ex.Message}");
                            }
                        }

                        // Example: delete the first message (optional – uncomment to enable).
                        /*
                        if (messageInfos.Count > 0)
                        {
                            try
                            {
                                string uriToDelete = messageInfos[0].UniqueUri;
                                ewsClient.DeleteItem(uriToDelete, DeletionOptions.DeletePermanently);
                                Console.WriteLine($"Deleted message: {uriToDelete}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to delete message: {ex.Message}");
                            }
                        }
                        */
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
