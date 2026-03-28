using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open PST with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Delete a specific folder named "OldFolder" if it exists
                try
                {
                    FolderInfo oldFolder = pst.RootFolder.GetSubFolder("OldFolder");
                    if (oldFolder != null)
                    {
                        // Delete the folder using its entry ID string
                        pst.DeleteItem(oldFolder.EntryIdString);
                        Console.WriteLine("Folder 'OldFolder' deleted.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                }

                // Delete a specific message with subject "DeleteMe" from Inbox
                try
                {
                    FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    if (inbox != null)
                    {
                        List<string> messagesToDelete = new List<string>();
                        foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                        {
                            if (msgInfo.Subject != null && msgInfo.Subject.Equals("DeleteMe", StringComparison.OrdinalIgnoreCase))
                            {
                                messagesToDelete.Add(msgInfo.EntryIdString);
                            }
                        }

                        if (messagesToDelete.Count > 0)
                        {
                            // Delete messages using their entry ID strings
                            inbox.DeleteChildItems(messagesToDelete);
                            Console.WriteLine($"{messagesToDelete.Count} message(s) with subject 'DeleteMe' deleted.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
