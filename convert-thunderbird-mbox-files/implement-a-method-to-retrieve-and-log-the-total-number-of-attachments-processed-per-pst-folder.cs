using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstAttachmentCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file (placeholder if not present)
                string pstPath = "storage.pst";

                // Create a minimal PST file if it does not exist
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through each subfolder of the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        int totalAttachmentsInFolder = 0;

                        // Enumerate all messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            // Extract the full message
                            MapiMessage message = pst.ExtractMessage(messageInfo);
                            if (message?.Attachments != null)
                            {
                                totalAttachmentsInFolder += message.Attachments.Count;
                            }
                        }

                        // Log the total number of attachments for the folder
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total attachments: {totalAttachmentsInFolder}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
