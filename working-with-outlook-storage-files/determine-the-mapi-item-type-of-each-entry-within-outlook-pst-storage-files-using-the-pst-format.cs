using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace DetermineMapiItemTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file (replace with your actual file path)
                string pstPath = "storage.pst";

                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Directory where extracted messages will be saved
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all top‑level folders
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate each message in the folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            // Extract the MAPI message from the PST
                            MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                            // Determine the MAPI item type via the MessageClass property
                            string messageClass = mapiMessage.MessageClass ?? "Unknown";
                            Console.WriteLine($"MAPI Message Class: {messageClass}");

                            // Create a safe filename based on the subject
                            string safeSubject = string.IsNullOrWhiteSpace(mapiMessage.Subject) ? "Untitled" : mapiMessage.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }

                            // Truncate if the filename is excessively long
                            if (safeSubject.Length > 100)
                                safeSubject = safeSubject.Substring(0, 100);

                            string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                            try
                            {
                                mapiMessage.Save(msgPath);
                                Console.WriteLine($"Saved message to: {msgPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message '{safeSubject}': {ex.Message}");
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
}
