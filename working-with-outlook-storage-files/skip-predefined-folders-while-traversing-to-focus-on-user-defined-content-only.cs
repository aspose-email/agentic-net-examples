using System;
using System.IO;
using System.Collections.Generic;
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

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Prepare output directory for saved messages.
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Collect EntryIdString values of all predefined folders to skip them.
                HashSet<string> predefinedFolderIds = new HashSet<string>();
                foreach (StandardIpmFolder predefined in Enum.GetValues(typeof(StandardIpmFolder)))
                {
                    try
                    {
                        FolderInfo predefinedFolder = pst.GetPredefinedFolder(predefined);
                        if (predefinedFolder != null && !string.IsNullOrEmpty(predefinedFolder.EntryIdString))
                        {
                            predefinedFolderIds.Add(predefinedFolder.EntryIdString);
                        }
                    }
                    catch
                    {
                        // Some predefined folders may not exist; ignore.
                    }
                }

                // Traverse user‑defined subfolders under the root folder.
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Skip predefined folders.
                    if (predefinedFolderIds.Contains(folderInfo.EntryIdString))
                    {
                        continue;
                    }

                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder.
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the full message as a MapiMessage.
                        using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                        {
                            // Prepare a safe filename.
                            string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(invalidChar, '_');
                            }

                            // Truncate if too long for file system.
                            const int maxFileNameLength = 200;
                            if (safeSubject.Length > maxFileNameLength)
                            {
                                safeSubject = safeSubject.Substring(0, maxFileNameLength);
                            }

                            string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                            try
                            {
                                // Save the message to disk.
                                mapiMsg.Save(outputPath);
                                Console.WriteLine($"Saved message to '{outputPath}'.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message '{mapiMsg.Subject}': {ex.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
