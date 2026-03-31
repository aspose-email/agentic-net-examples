using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Subscribe to the ItemMoved event to receive updates
                pst.ItemMoved += OnItemMoved;

                // Get the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Ensure a target folder exists (e.g., "Archive")
                FolderInfo archiveFolder = null;
                foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                {
                    if (string.Equals(subFolder.DisplayName, "Archive", StringComparison.OrdinalIgnoreCase))
                    {
                        archiveFolder = subFolder;
                        break;
                    }
                }
                if (archiveFolder == null)
                {
                    try
                    {
                        archiveFolder = rootFolder.AddSubFolder("Archive");
                        Console.WriteLine("Created folder 'Archive'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating folder: {ex.Message}");
                        return;
                    }
                }

                // Find the first message in the Inbox (or root) to move
                MessageInfo messageToMove = null;
                foreach (MessageInfo msgInfo in rootFolder.EnumerateMessages())
                {
                    messageToMove = msgInfo;
                    break;
                }

                if (messageToMove == null)
                {
                    Console.WriteLine("No messages found to move.");
                }
                else
                {
                    try
                    {
                        // Move the message to the Archive folder
                        pst.MoveItem(messageToMove, archiveFolder);
                        Console.WriteLine($"Moved message with Subject: '{messageToMove.Subject}' to 'Archive'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error moving message: {ex.Message}");
                    }
                }

                // Unsubscribe from the event (optional, as disposing will clean up)
                pst.ItemMoved -= OnItemMoved;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    // Event handler for ItemMoved event
    private static void OnItemMoved(object sender, ItemMovedEventArgs e)
    {
        try
        {
            // Use EntryId property (SourceId does not exist)
            string entryId = e.EntryId;
            string destinationFolderName = e.DestinationFolder != null ? e.DestinationFolder.DisplayName : "Unknown";
            Console.WriteLine($"Item moved. EntryId: {entryId}, Destination Folder: {destinationFolderName}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in ItemMoved handler: {ex.Message}");
        }
    }
}
