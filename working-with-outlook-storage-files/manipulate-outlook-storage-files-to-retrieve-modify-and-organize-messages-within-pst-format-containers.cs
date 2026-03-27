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

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Ensure the PST is writable
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("Error: PST file is read‑only.");
                    return;
                }

                // Get the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Create (or get) a folder named "Processed"
                FolderInfo processedFolder = null;
                foreach (FolderInfo sub in rootFolder.GetSubFolders())
                {
                    if (sub.DisplayName.Equals("Processed", StringComparison.OrdinalIgnoreCase))
                    {
                        processedFolder = sub;
                        break;
                    }
                }
                if (processedFolder == null)
                {
                    processedFolder = rootFolder.AddSubFolder("Processed");
                }

                // Iterate through messages in the root folder
                foreach (MessageInfo msgInfo in rootFolder.EnumerateMessages())
                {
                    // Extract the full message
                    using (MapiMessage message = pst.ExtractMessage(msgInfo))
                    {
                        // Check subject and modify if needed
                        if (!string.IsNullOrEmpty(message.Subject) && message.Subject.Contains("Test"))
                        {
                            message.Subject = "[Processed] " + message.Subject;

                            // Add the modified message to the "Processed" folder
                            string newEntryId = processedFolder.AddMessage(message);

                            // Delete the original message from the root folder
                            rootFolder.DeleteChildItem(msgInfo.EntryId);
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
