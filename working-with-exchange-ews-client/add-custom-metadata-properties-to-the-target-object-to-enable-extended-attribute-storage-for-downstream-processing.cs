using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.PersonalInfo;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                if (serviceUrl.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("EWS connection parameters are placeholders. Skipping external call.");
                    return;
                }

                // Create and connect the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Get mailbox information.
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                        // Get the Inbox folder URI.
                        string inboxUri = mailboxInfo.InboxUri;

                        // List messages in the Inbox.
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                        Console.WriteLine($"Found {messages.Count} message(s) in Inbox.");

                        if (messages.Count > 0)
                        {
                            // Fetch the first message.
                            ExchangeMessageInfo firstInfo = messages[0];
                            MailMessage fetchedMessage = client.FetchMessage(firstInfo.UniqueUri);

                            // Save the fetched message to a local file.
                            string outputPath = "FetchedMessage.eml";
                            try
                            {
                                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }
                                fetchedMessage.Save(outputPath);
                                Console.WriteLine($"Message saved to {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                                return;
                            }

                            // Ensure a sample subfolder exists under Inbox.
                            string sampleFolderName = "AsposeSampleFolder";
                            ExchangeFolderInfo sampleFolder;
                            if (!client.FolderExists(inboxUri, sampleFolderName))
                            {
                                sampleFolder = client.CreateFolder(inboxUri, sampleFolderName);
                                Console.WriteLine($"Created folder '{sampleFolderName}'.");
                            }
                            else
                            {
                                // Retrieve existing folder info.
                                sampleFolder = client.GetFolderInfo($"{inboxUri}/{sampleFolderName}");
                                Console.WriteLine($"Folder '{sampleFolderName}' already exists.");
                            }

                            // Move the fetched message to the sample folder.
                            client.MoveItem(firstInfo.UniqueUri, sampleFolder.Uri);
                            Console.WriteLine("Message moved to the sample folder.");

                            // Delete the moved message (move to Deleted Items).
                            var deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                            client.DeleteItem(firstInfo.UniqueUri, deleteOptions);
                            Console.WriteLine("Message deleted (moved to Deleted Items).");
                        }
                        else
                        {
                            Console.WriteLine("No messages found in Inbox.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
}
