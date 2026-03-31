using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace ExtractPstRecipients
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal placeholder if missing.
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        using (PersonalStorage placeholderPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                        {
                            // Create a default Inbox folder.
                            placeholderPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                        }

                        Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                        return;
                    }
                }

                // Open the PST file.
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Breadth‑first traversal of all folders.
                    Queue<FolderInfo> folderQueue = new Queue<FolderInfo>();
                    folderQueue.Enqueue(pst.RootFolder);

                    while (folderQueue.Count > 0)
                    {
                        FolderInfo currentFolder = folderQueue.Dequeue();

                        // Enumerate messages in the current folder.
                        foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                        {
                            try
                            {
                                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                                {
                                    if (message?.Recipients != null)
                                    {
                                        foreach (MapiRecipient recipient in message.Recipients)
                                        {
                                            Console.WriteLine($"Recipient: {recipient.EmailAddress}");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error processing message '{messageInfo?.Subject}': {ex.Message}");
                            }
                        }

                        // Enqueue subfolders for further processing.
                        foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                        {
                            folderQueue.Enqueue(subFolder);
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
