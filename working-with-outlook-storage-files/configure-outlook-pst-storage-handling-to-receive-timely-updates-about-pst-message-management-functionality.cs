using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at: {pstPath}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file – {createEx.Message}");
                    return;
                }
            }

            // Open the PST file within a using block to guarantee disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Subscribe to the ItemMoved event to receive updates about item moves
                pst.ItemMoved += (object sender, ItemMovedEventArgs e) =>
                {
                    // Use verified members of ItemMovedEventArgs
                    FolderInfo destinationFolder = e.DestinationFolder;
                    string entryId = e.EntryId;
                    bool isFolder = e.IsFolder;
                    bool isMessage = e.IsMessage;

                    Console.WriteLine("ItemMoved event triggered:");
                    Console.WriteLine($"  Destination Folder: {destinationFolder?.DisplayName ?? "null"}");
                    Console.WriteLine($"  EntryId: {entryId}");
                    Console.WriteLine($"  IsFolder: {isFolder}");
                    Console.WriteLine($"  IsMessage: {isMessage}");
                };

                // Retrieve standard folders
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                FolderInfo deletedItemsFolder = pst.GetPredefinedFolder(StandardIpmFolder.DeletedItems);

                // Move the first message from Inbox to Deleted Items to demonstrate the event
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    try
                    {
                        pst.MoveItem(messageInfo, deletedItemsFolder);
                        Console.WriteLine($"Moved message \"{messageInfo.Subject}\" to Deleted Items.");
                    }
                    catch (Exception moveEx)
                    {
                        Console.Error.WriteLine($"Error moving message – {moveEx.Message}");
                    }
                    // Only move one message for the demo
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
