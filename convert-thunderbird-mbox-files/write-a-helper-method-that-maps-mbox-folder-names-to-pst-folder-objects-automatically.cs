using Aspose.Email.Calendar;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";
            string pstPath = "output.pst";

            // Ensure MBOX file exists; create minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (var writer = new StreamWriter(mboxPath))
                    {
                        // Minimal placeholder: a single empty message separator
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                    }
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST (creates PST file)
            PersonalStorage pst;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Example folder name mapping
                var mboxFolderNames = new List<string>
                {
                    "Inbox",
                    "Sent",
                    "Drafts",
                    "Trash",
                    "CustomFolder"
                };

                foreach (var mboxFolder in mboxFolderNames)
                {
                    FolderInfo pstFolder = GetOrCreatePstFolder(pst, mboxFolder);
                    Console.WriteLine($"Mapped MBOX folder '{mboxFolder}' to PST folder '{pstFolder.DisplayName}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper method that maps an MBOX folder name to a PST FolderInfo.
    // Returns a predefined folder for known names or creates a new subfolder under the root.
    static FolderInfo GetOrCreatePstFolder(PersonalStorage pst, string mboxFolderName)
    {
        if (pst == null) throw new ArgumentNullException(nameof(pst));
        if (string.IsNullOrEmpty(mboxFolderName)) throw new ArgumentException("Folder name cannot be null or empty.", nameof(mboxFolderName));

        switch (mboxFolderName.Trim().ToLowerInvariant())
        {
            case "inbox":
                return pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
            case "sent":
            case "sent items":
                return pst.GetPredefinedFolder(StandardIpmFolder.SentItems);
            case "drafts":
                return pst.GetPredefinedFolder(StandardIpmFolder.Drafts);
            case "trash":
            case "deleted items":
                return pst.GetPredefinedFolder(StandardIpmFolder.DeletedItems);
            case "outbox":
                return pst.GetPredefinedFolder(StandardIpmFolder.Outbox);
            case "junk":
            case "junk email":
                return pst.GetPredefinedFolder(StandardIpmFolder.JunkEmail);
            case "calendar":
            case "appointments":
                return pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
            default:
                // Look for an existing subfolder with the same display name
                foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(subFolder.DisplayName, mboxFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        return subFolder;
                    }
                }
                // Create a new subfolder under the root
                return pst.RootFolder.AddSubFolder(mboxFolderName);
        }
    }
}
