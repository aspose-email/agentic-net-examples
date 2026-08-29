using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            PersonalStorage pstStorage;
            try
            {
                pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Verify the PST can be opened and read
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Retrieve total items count
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItems}");

                    // Iterate through each subfolder in the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"  Items: {folderInfo.ContentCount}");
                        Console.WriteLine($"  Unread: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages in the folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"  Subject: {messageInfo.Subject}");

                            // Extract the full message to ensure it can be read
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Extraction succeeded; no further action needed
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open or read PST: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose the PST storage returned by the conversion if not null
                pstStorage?.Dispose();
            }

            Console.WriteLine("PST file validated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
