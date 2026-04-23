using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file paths
            string inputPstPath = "input.pst";
            string outputPstPath = "output.pst";

            // Ensure input PST exists; create a minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    // Create an empty Unicode PST file as a placeholder
                    PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode);
                    Console.Error.WriteLine($"Input PST not found. Created placeholder at '{inputPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Copy input PST to output PST to preserve original while modifying
            try
            {
                File.Copy(inputPstPath, outputPstPath, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to copy PST file: {ex.Message}");
                return;
            }

            // Open the PST file for read/write
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(outputPstPath))
                {
                    // Iterate through all subfolders starting from the root
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        ProcessFolder(pst, folderInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively process a folder and its subfolders
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folderInfo)
    {
        // Process each message in the current folder
        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
        {
            try
            {
                using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                {
                    // Check if the message requests a read receipt
                    if (msg.ReadReceiptRequested)
                    {
                        // Add a custom property to preserve the read receipt request
                        string propertyName = "ReadReceiptRequested";
                        byte[] propertyValue = Encoding.Unicode.GetBytes("true");
                        msg.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyValue, propertyName);
                    }

                    // Update the message in the PST with modified properties
                    pst.ChangeMessage(messageInfo.EntryIdString, msg.Properties);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {ex.Message}");
                // Continue with next message
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folderInfo.GetSubFolders())
        {
            ProcessFolder(pst, subFolder);
        }
    }
}
