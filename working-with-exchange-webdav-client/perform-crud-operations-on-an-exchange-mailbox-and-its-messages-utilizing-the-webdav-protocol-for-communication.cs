using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace ExchangeWebDavSample
{
    class Program
    {
        static void Main()
        {
            // Replace with actual Exchange WebDAV endpoint and credentials.
            string mailboxUri = "https://exchange.example.com/exchange/username";
            string username = "username";
            string password = "password";

            try
            {
                // Initialize the Exchange WebDAV client.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // -----------------------------------------------------------------
                    // CREATE: Create a new folder under the Inbox.
                    // -----------------------------------------------------------------
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;
                    string newFolderName = "SampleFolder";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Ensure the folder does not already exist.
                    if (!client.FolderExists(inboxUri, newFolderName, out ExchangeFolderInfo existingFolder))
                    {
                        client.CreateFolder(inboxUri, newFolderName);
                        Console.WriteLine($"Folder '{newFolderName}' created.");
                    }
                    else
                    {
                        Console.WriteLine($"Folder '{newFolderName}' already exists.");
                    }

                    // Retrieve the full URI of the newly created folder.
                    ExchangeFolderInfo newFolderInfo = client.GetFolderInfo($"{inboxUri}/{newFolderName}");
                    string newFolderUri = newFolderInfo.Uri;

                    // -----------------------------------------------------------------
                    // CREATE: Append a new mail message to the folder.
                    // -----------------------------------------------------------------
                    MailMessage mailMessage = new MailMessage("sender@example.com", "receiver@example.com", "Test Subject", "Test Body");
                    client.AppendMessage(newFolderUri, mailMessage);
                    Console.WriteLine("Message appended to the folder.");

                    // -----------------------------------------------------------------
                    // READ: List messages in the folder and fetch the first one.
                    // -----------------------------------------------------------------
                    ExchangeMessageInfoCollection messages = client.ListMessages(newFolderUri);
                    if (messages.Count > 0)
                    {
                        ExchangeMessageInfo firstMessageInfo = messages[0];
                        MailMessage fetchedMessage = client.FetchMessage(firstMessageInfo.UniqueUri);
                        Console.WriteLine($"Fetched message subject: {fetchedMessage.Subject}");

                        // -----------------------------------------------------------------
                        // UPDATE: Move the message to Deleted Items instead of deleting it.
                        // -----------------------------------------------------------------
                        string deletedItemsUri = mailboxInfo.DeletedItemsUri;
                        client.MoveMessage(firstMessageInfo, deletedItemsUri);
                        Console.WriteLine("Message moved to Deleted Items.");
                    }
                    else
                    {
                        Console.WriteLine("No messages found in the folder.");
                    }

                    // -----------------------------------------------------------------
                    // DELETE: Remove the created folder.
                    // -----------------------------------------------------------------
                    client.DeleteFolder(newFolderUri);
                    Console.WriteLine($"Folder '{newFolderName}' deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // In a real application, consider logging the full exception details.
            }
        }
    }
}
