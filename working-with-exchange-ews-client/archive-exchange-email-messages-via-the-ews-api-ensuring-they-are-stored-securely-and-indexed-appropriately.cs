using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;
using Aspose.Email.Tools.Search;
using Aspose.Email.PersonalInfo;

namespace AsposeEmailEwsSample
{
    // Sample author: Aspose.Email .NET documentation
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // -----------------------------------------------------------------
                // Configuration – replace with real values or keep placeholders.
                // -----------------------------------------------------------------
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "your_username@example.com";
                string password = "your_password";
                string domain = ""; // optional, leave empty if not needed

                // Guard against placeholder credentials.
                if (string.IsNullOrWhiteSpace(mailboxUri) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    mailboxUri.Contains("your_") ||
                    username.Contains("your_") ||
                    password.Contains("your_"))
                {
                    Console.Error.WriteLine("Please provide valid mailbox URI, username and password. Skipping EWS operations.");
                    return;
                }

                // -----------------------------------------------------------------
                // Create EWS client inside a using block to ensure proper disposal.
                // -----------------------------------------------------------------
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, domain))
                {
                    // -----------------------------------------------------------------
                    // Get mailbox information and the Inbox folder URI.
                    // -----------------------------------------------------------------
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // -----------------------------------------------------------------
                    // List messages in the Inbox.
                    // -----------------------------------------------------------------
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                    Console.WriteLine($"Inbox contains {messages.Count} message(s).");

                    if (messages.Count == 0)
                    {
                        Console.WriteLine("No messages to process.");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // Process the first message: fetch, display subject, and save to file.
                    // -----------------------------------------------------------------
                    ExchangeMessageInfo firstInfo = messages[0];
                    Console.WriteLine($"Fetching message: Subject = \"{firstInfo.Subject}\"");

                    MailMessage fetchedMessage = client.FetchMessage(firstInfo.UniqueUri);
                    string outputDir = "FetchedMessages";
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    string emlPath = Path.Combine(outputDir, "Message1.eml");
                    try
                    {
                        fetchedMessage.Save(emlPath);
                        Console.WriteLine($"Message saved to: {emlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }

                    // -----------------------------------------------------------------
                    // Ensure a subfolder named "Processed" exists under the Inbox.
                    // -----------------------------------------------------------------
                    string processedFolderName = "Processed";
                    ExchangeFolderInfo processedFolderInfo;
                    bool folderExists = client.FolderExists(inboxUri, processedFolderName, out processedFolderInfo);
                    if (!folderExists)
                    {
                        processedFolderInfo = client.CreateFolder(inboxUri, processedFolderName);
                        Console.WriteLine($"Created folder \"{processedFolderName}\" with URI: {processedFolderInfo.Uri}");
                    }
                    else
                    {
                        Console.WriteLine($"Folder \"{processedFolderName}\" already exists with URI: {processedFolderInfo.Uri}");
                    }

                    // -----------------------------------------------------------------
                    // Move the fetched message to the "Processed" folder.
                    // -----------------------------------------------------------------
                    try
                    {
                        client.MoveItem(firstInfo.UniqueUri, processedFolderInfo.Uri);
                        Console.WriteLine("Message moved to the \"Processed\" folder.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message: {ex.Message}");
                    }

                    // -----------------------------------------------------------------
                    // Delete the moved message permanently.
                    // -----------------------------------------------------------------
                    try
                    {
                        // Retrieve the moved message's URI.
                        ExchangeMessageInfoCollection movedMessages = client.ListMessages(processedFolderInfo.Uri);
                        if (movedMessages.Count > 0)
                        {
                            string movedMessageUri = movedMessages[0].UniqueUri;
                            client.DeleteItem(movedMessageUri, DeletionOptions.DeletePermanently);
                            Console.WriteLine("Moved message deleted permanently.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete moved message: {ex.Message}");
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
