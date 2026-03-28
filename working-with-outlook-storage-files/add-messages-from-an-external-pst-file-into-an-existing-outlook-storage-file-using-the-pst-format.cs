using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string sourcePstPath = "source.pst";
            string destinationPstPath = "dest.pst";

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {sourcePstPath}");
                return;
            }

            // Verify destination PST exists; create a minimal one if missing
            if (!File.Exists(destinationPstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(destinationPstPath, FileFormatVersion.Unicode))
                    {
                        // Optionally create default folders
                        createdPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating destination PST: {ex.Message}");
                    return;
                }
            }

            // Open source PST (read‑only) and destination PST (writable)
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath, false))
            using (PersonalStorage destinationPst = PersonalStorage.FromFile(destinationPstPath, true))
            {
                // Get (or create) the Inbox folder in the destination PST
                FolderInfo destinationInbox = destinationPst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Iterate through each subfolder in the source PST
                foreach (FolderInfo sourceFolder in sourcePst.RootFolder.GetSubFolders())
                {
                    // Enumerate all messages in the current source folder
                    foreach (MessageInfo messageInfo in sourceFolder.EnumerateMessages())
                    {
                        // Extract the message as a MapiMessage
                        using (MapiMessage mapiMessage = sourcePst.ExtractMessage(messageInfo))
                        {
                            // Add the message to the destination Inbox
                            destinationInbox.AddMessage(mapiMessage);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
