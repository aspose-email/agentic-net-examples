using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "archive.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Use the root folder for traversal
                    FolderInfo rootFolder = pst.RootFolder;

                    // HashSet to track unique Transport Message IDs
                    HashSet<string> messageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    bool duplicateFound = false;

                    // Recursive traversal of folders
                    void ProcessFolder(FolderInfo folder)
                    {
                        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                        {
                            try
                            {
                                using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                                {
                                    string transportId = mapiMsg.InternetMessageId;
                                    if (string.IsNullOrEmpty(transportId))
                                    {
                                        transportId = "(no ID)";
                                    }

                                    if (!messageIds.Add(transportId))
                                    {
                                        Console.WriteLine($"Duplicate Transport Message ID detected: {transportId}");
                                        duplicateFound = true;
                                    }
                                }
                            }
                            catch (Exception exMsg)
                            {
                                Console.Error.WriteLine($"Error processing message: {exMsg.Message}");
                            }
                        }

                        // Process subfolders recursively
                        foreach (FolderInfo subFolder in folder.GetSubFolders())
                        {
                            ProcessFolder(subFolder);
                        }
                    }

                    ProcessFolder(rootFolder);

                    if (!duplicateFound)
                    {
                        Console.WriteLine("All transport message IDs are unique.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
