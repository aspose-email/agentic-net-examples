using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory for PST: {dirEx.Message}");
                    return;
                }
            }

            // Convert MBOX to PST inside a try/catch to handle conversion errors
            try
            {
                // MailStorageConverter is in Aspose.Email.Storage namespace
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Log total items in the PST store
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItems}");

                    // Iterate through each subfolder of the root folder
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName}");
                        // ContentCount gives the number of messages in the folder
                        Console.WriteLine($"Messages processed in folder: {folder.ContentCount}");
                    }
                }
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
