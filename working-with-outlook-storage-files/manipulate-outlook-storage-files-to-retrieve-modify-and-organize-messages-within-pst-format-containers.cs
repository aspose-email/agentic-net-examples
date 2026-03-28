using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal one if missing
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                        {
                            // No additional setup required for an empty PST
                        }
                        Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                        return;
                    }
                }

                // Open the PST file for read/write operations
                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // List existing top‑level folders
                        Console.WriteLine("Existing top‑level folders:");
                        foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                        {
                            Console.WriteLine($"- {folder.DisplayName} (Items: {folder.ContentCount})");
                        }

                        // Create a new subfolder under the root folder
                        FolderInfo myFolder;
                        try
                        {
                            myFolder = pst.RootFolder.AddSubFolder("MyFolder");
                            Console.WriteLine("Created folder 'MyFolder'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating folder: {ex.Message}");
                            return;
                        }

                        // Add a simple message to the new folder
                        MapiMessage newMessage = new MapiMessage(
                            "sender@example.com",
                            "receiver@example.com",
                            "Sample Subject",
                            "This is a sample message body.");
                        try
                        {
                            string entryId = myFolder.AddMessage(newMessage);
                            Console.WriteLine($"Added message to 'MyFolder' (EntryId: {entryId}).");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error adding message: {ex.Message}");
                            return;
                        }

                        // Move the first message from the root folder (if any) to the new folder
                        MessageInfo firstMessageInfo = null;
                        foreach (MessageInfo info in pst.RootFolder.EnumerateMessages())
                        {
                            firstMessageInfo = info;
                            break;
                        }

                        if (firstMessageInfo != null)
                        {
                            try
                            {
                                pst.MoveItem(firstMessageInfo, myFolder);
                                Console.WriteLine($"Moved message '{firstMessageInfo.Subject}' to 'MyFolder'.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error moving message: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the root folder to move.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
