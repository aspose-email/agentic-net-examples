using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace MboxToPstIntegrationTest
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input MBOX and output PST file paths
                string mboxFilePath = "sample.mbox";
                string pstFilePath = "output.pst";

                // Ensure the input MBOX file exists; create a minimal placeholder if missing
                if (!File.Exists(mboxFilePath))
                {
                    try
                    {
                        // Create an empty MBOX file (valid but contains no messages)
                        File.WriteAllText(mboxFilePath, string.Empty);
                        Console.WriteLine($"Created placeholder MBOX file at '{mboxFilePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                        return;
                    }
                }

                // Ensure the directory for the PST file exists
                string pstDirectory = Path.GetDirectoryName(pstFilePath);
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

                // Perform the conversion from MBOX to PST
                try
                {
                    // This method creates the PST file at the specified location
                    MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                    return;
                }

                // Verify that the PST file was created
                if (!File.Exists(pstFilePath))
                {
                    Console.Error.WriteLine("PST file was not created.");
                    return;
                }

                // Open the created PST to inspect its contents
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Retrieve total items count from the PST store
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItems}");

                    // Iterate through each subfolder of the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"  Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"  Unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages within the folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"  Subject: {messageInfo.Subject}");
                        }

                        // Count subfolders
                        int subFolderCount = folderInfo.GetSubFolders().Count;
                        Console.WriteLine($"  Subfolder count: {subFolderCount}");
                    }
                }

                Console.WriteLine("MBOX to PST conversion test completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
