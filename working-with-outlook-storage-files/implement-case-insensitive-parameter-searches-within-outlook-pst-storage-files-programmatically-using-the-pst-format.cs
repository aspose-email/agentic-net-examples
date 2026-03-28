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
            // Path to the PST file (adjust as needed)
            string pstPath = "sample.pst";

            // Verify that the PST file exists before attempting to open it
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Define the case‑insensitive search term
            string searchTerm = "meeting";

            // Open the PST file safely
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Enumerate messages within the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        // Extract the full message as a MapiMessage
                        using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                        {
                            // Perform a case‑insensitive search on the Subject field
                            if (!string.IsNullOrEmpty(msg.Subject) &&
                                msg.Subject.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                                Console.WriteLine($"Subject: {msg.Subject}");
                                Console.WriteLine($"From: {msg.SenderName}");
                                Console.WriteLine(new string('-', 40));
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
