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
            string sourcePstPath = "source.pst";
            string destinationPstPath = "dest.pst";

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: Source PST file not found – {sourcePstPath}");
                return;
            }

            // Ensure destination PST exists; create if missing
            if (!File.Exists(destinationPstPath))
            {
                try
                {
                    PersonalStorage.Create(destinationPstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created destination PST: {destinationPstPath}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating destination PST: {createEx.Message}");
                    return;
                }
            }

            // Open both PST files with write access
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath, true))
            using (PersonalStorage destinationPst = PersonalStorage.FromFile(destinationPstPath, true))
            {
                // Get the Inbox folder from the source PST
                FolderInfo sourceInbox = null;
                foreach (FolderInfo folder in sourcePst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(folder.DisplayName, "Inbox", StringComparison.OrdinalIgnoreCase))
                    {
                        sourceInbox = folder;
                        break;
                    }
                }

                if (sourceInbox == null)
                {
                    Console.Error.WriteLine("Error: Source PST does not contain an Inbox folder.");
                    return;
                }

                // Get (or create) the Inbox folder in the destination PST
                FolderInfo destinationInbox = destinationPst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Iterate through each message in the source Inbox and add to destination Inbox
                foreach (MessageInfo messageInfo in sourceInbox.EnumerateMessages())
                {
                    using (MapiMessage sourceMessage = sourcePst.ExtractMessage(messageInfo))
                    {
                        string addedEntryId = destinationInbox.AddMessage(sourceMessage);
                        Console.WriteLine($"Added message: {messageInfo.Subject} (EntryId: {addedEntryId})");
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
