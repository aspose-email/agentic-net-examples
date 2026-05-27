using Aspose.Email.Mapi;
using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create the Journals folder
                        FolderInfo journalFolder = createdPst.CreatePredefinedFolder("Journals", StandardIpmFolder.Journal);
                        // Add a dummy message to the Journals folder
                        MapiMessage dummyMessage = new MapiMessage("author@example.com", "recipient@example.com", "Dummy Journal", "This is a dummy journal entry.");
                        journalFolder.AddMessage(dummyMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for read/write operations
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Attempt to get the Journals folder; create if it does not exist
                    FolderInfo journalFolder;
                    try
                    {
                        journalFolder = pst.GetPredefinedFolder(StandardIpmFolder.Journal);
                    }
                    catch (Exception)
                    {
                        // Folder not found; nothing to delete
                        Console.WriteLine("Journals folder does not exist.");
                        return;
                    }

                    // Delete all messages inside the Journals folder
                    List<string> entryIdsToDelete = new List<string>();
                    foreach (MessageInfo messageInfo in journalFolder.EnumerateMessages())
                    {
                        entryIdsToDelete.Add(messageInfo.EntryIdString);
                    }

                    if (entryIdsToDelete.Count > 0)
                    {
                        journalFolder.DeleteChildItems(entryIdsToDelete);
                    }

                    // Delete the Journals folder itself using its entry ID
                    string journalFolderEntryId = journalFolder.EntryIdString;
                    pst.DeleteItem(journalFolderEntryId);

                    // Verify that the Journals folder no longer exists
                    bool journalFolderExists;
                    try
                    {
                        pst.GetPredefinedFolder(StandardIpmFolder.Journal);
                        journalFolderExists = true;
                    }
                    catch (Exception)
                    {
                        journalFolderExists = false;
                    }

                    Console.WriteLine(journalFolderExists
                        ? "Failed to delete Journals folder."
                        : "Journals folder successfully deleted and no journal items remain.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
