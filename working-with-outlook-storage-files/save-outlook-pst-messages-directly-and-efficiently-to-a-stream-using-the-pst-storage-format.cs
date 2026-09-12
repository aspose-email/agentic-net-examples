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
            string pstPath = "storage.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create an empty PST with Unicode format
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve total items count via the Store property
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");

                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Convert the byte[] EntryId to a Base64 string as required by SaveMessageToStream
                        string entryIdString = Convert.ToBase64String(messageInfo.EntryId);

                        // Save the message directly to a memory stream
                        using (MemoryStream messageStream = new MemoryStream())
                        {
                            pst.SaveMessageToStream(entryIdString, messageStream);
                            // Reset stream position for any further processing
                            messageStream.Position = 0;
                            Console.WriteLine($"Saved message to stream (size: {messageStream.Length} bytes).");
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
