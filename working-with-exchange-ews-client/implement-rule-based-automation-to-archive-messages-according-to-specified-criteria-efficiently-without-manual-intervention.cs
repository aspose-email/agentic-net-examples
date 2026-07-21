using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange server connection parameters
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create EWS client (IDisposable)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // -------------------- List messages in Inbox --------------------
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages(inboxUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                        return;
                    }

                    if (messageInfos != null && messageInfos.Count > 0)
                    {
                        foreach (var msgInfo in messageInfos)
                        {
                            try
                            {
                                // Fetch full message and display subject
                                using (MailMessage mail = client.FetchMessage(msgInfo.UniqueUri))
                                {
                                    Console.WriteLine($"Subject: {mail.Subject}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error fetching message {msgInfo.UniqueUri}: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No messages found in Inbox.");
                    }

                    // -------------------- Create a subfolder under Inbox --------------------
                    string newFolderName = "SampleFolder";

                    // Skip external calls when placeholder credentials are used
                    if (username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    ExchangeFolderInfo newFolderInfo = null;
                    try
                    {
                        bool folderExists = client.FolderExists(inboxUri, newFolderName, out newFolderInfo);
                        if (!folderExists)
                        {
                            newFolderInfo = client.CreateFolder(inboxUri, newFolderName, null, null);
                            Console.WriteLine($"Created folder: {newFolderInfo.Uri}");
                        }
                        else
                        {
                            Console.WriteLine($"Folder already exists: {newFolderInfo.Uri}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error handling folder '{newFolderName}': {ex.Message}");
                    }

                    // -------------------- Move first message to the new folder --------------------
                    if (messageInfos != null && messageInfos.Count > 0 && newFolderInfo != null)
                    {
                        string firstMessageUri = messageInfos[0].UniqueUri;
                        try
                        {
                            client.MoveItem(firstMessageUri, newFolderInfo.Uri);
                            Console.WriteLine($"Moved message to {newFolderInfo.Uri}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error moving message: {ex.Message}");
                        }
                    }

                    // -------------------- Cleanup: delete the created folder --------------------
                    if (newFolderInfo != null)
                    {
                        try
                        {
                            client.DeleteFolder(newFolderInfo.Uri, true);
                            Console.WriteLine($"Deleted folder: {newFolderInfo.Uri}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                        }
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
