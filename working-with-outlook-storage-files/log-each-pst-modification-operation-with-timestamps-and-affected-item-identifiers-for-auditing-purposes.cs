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
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // PST created successfully.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("PST file is read‑only. Modifications are not allowed.");
                    return;
                }

                // Get the Inbox folder (or create it if missing).
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch
                {
                    // Folder does not exist; create it.
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // ---------- Add a new message ----------
                MapiMessage newMessage = new MapiMessage(
                    "alice@example.com",
                    "bob@example.com",
                    "Sample Subject",
                    "This is a sample message body.");

                string addedEntryId;
                try
                {
                    addedEntryId = inboxFolder.AddMapiMessageItem(newMessage);
                    Console.WriteLine($"{DateTime.Now:u} - Added message, EntryId: {addedEntryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add message: {ex.Message}");
                    return;
                }

                // ---------- Update the added message ----------
                try
                {
                    MapiMessage extractedMessage = pst.ExtractMessage(addedEntryId);
                    extractedMessage.Subject = "Updated Subject";
                    inboxFolder.UpdateMessage(addedEntryId, extractedMessage);
                    Console.WriteLine($"{DateTime.Now:u} - Updated message, EntryId: {addedEntryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update message: {ex.Message}");
                    return;
                }

                // ---------- Delete the message ----------
                try
                {
                    pst.DeleteItem(addedEntryId);
                    Console.WriteLine($"{DateTime.Now:u} - Deleted message, EntryId: {addedEntryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete message: {ex.Message}");
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
