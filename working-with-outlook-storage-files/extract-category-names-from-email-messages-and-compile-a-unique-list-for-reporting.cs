using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file (replace with actual path if needed)
            string pstPath = "sample.pst";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a subfolder
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

                    // Create a sample message with categories
                    MapiMessage sampleMsg = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "Sample body of the message.");

                    // Assign categories (placeholder values)
                    sampleMsg.Categories = new[] { "Category1", "Category2" };

                    // Add the message to the folder
                    inbox.AddMessage(sampleMsg);
                }

                Console.WriteLine($"Placeholder PST file created at: {pstPath}");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Set to collect unique category names (case‑insensitive)
                HashSet<string> uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Process root folder messages
                ProcessFolder(pst.RootFolder, pst, uniqueCategories);

                // Iterate through all subfolders of the root folder
                foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(subFolder, pst, uniqueCategories);
                }

                // Report the collected unique categories
                Console.WriteLine("Unique categories found in PST messages:");
                foreach (string category in uniqueCategories)
                {
                    Console.WriteLine($"- {category}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, HashSet<string> uniqueCategories)
    {
        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            using (MapiMessage message = pst.ExtractMessage(msgInfo))
            {
                // Retrieve categories assigned to this message
                string[] messageCategories = message.Categories ?? Array.Empty<string>();

                // Add each category to the unique set
                foreach (string category in messageCategories)
                {
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        uniqueCategories.Add(category);
                    }
                }
            }
        }
    }
}
