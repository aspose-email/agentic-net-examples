using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string targetCategory = "Important";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Add a default folder so the PST is not empty
                    pstCreate.RootFolder.AddSubFolder("Inbox");
                }
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through all subfolders of the root folder
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    // Get all messages in the folder
                    MessageInfoCollection allMessages = folder.GetContents();

                    foreach (MessageInfo msgInfo in allMessages)
                    {
                        // Extract the full message to access its properties
                        using (MapiMessage mapiMessage = pst.ExtractMessage(msgInfo))
                        {
                            if (mapiMessage.Categories != null && Array.Exists(mapiMessage.Categories, c => string.Equals(c, targetCategory, StringComparison.OrdinalIgnoreCase)))
                            {
                                Console.WriteLine($"Folder: {folder.DisplayName} | Subject: {msgInfo.Subject} | Category: {targetCategory}");
                            }
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
