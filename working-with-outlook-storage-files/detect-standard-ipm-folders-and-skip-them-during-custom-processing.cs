using Aspose.Email.Calendar;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Create a minimal placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                Console.WriteLine($"PST file not found. Creating placeholder: {pstPath}");
                // Create a new PST file with Unicode format
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Add a root folder (Inbox) to make the PST valid
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");
                    // Optionally add a dummy message
                    // (Skipping actual message creation to keep it simple)
                }
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Standard IPM folder display names to skip
                HashSet<string> standardFolderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Inbox",
                    "Deleted Items",
                    "Outbox",
                    "Sent Items",
                    "Appointments",
                    "Contacts",
                    "Drafts",
                    "Journal",
                    "Notes",
                    "Tasks",
                    "Sync Issues",
                    "Junk Email",
                    "Unspecified",
                    "RSS Feeds"
                };

                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    if (standardFolderNames.Contains(folderInfo.DisplayName))
                    {
                        Console.WriteLine($"Skipping standard folder: {folderInfo.DisplayName}");
                        continue;
                    }

                    // Custom processing for non‑standard folders
                    Console.WriteLine($"Processing custom folder: {folderInfo.DisplayName}");

                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"  Message: {messageInfo.Subject}");
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
