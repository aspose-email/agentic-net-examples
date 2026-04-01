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
            string outputDir = "ExtractedMessages";

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Guard PST file existence; create minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Display total items count
                Console.WriteLine($"Total items in PST: {pst.Store.GetTotalItemsCount()}");

                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folder.DisplayName}");
                    Console.WriteLine($"  Items: {folder.ContentCount}");
                    Console.WriteLine($"  Unread: {folder.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        // Output selected metadata
                        Console.WriteLine($"  Subject: {msgInfo.Subject}");
                        Console.WriteLine($"  Sender: {msgInfo.SenderRepresentativeName}");
                        Console.WriteLine($"  To: {msgInfo.DisplayTo}");
                        Console.WriteLine($"  CC: {msgInfo.DisplayCC}");

                        // Extract full MAPI message
                        using (MapiMessage message = pst.ExtractMessage(msgInfo))
                        {
                            // Build a safe filename from the subject
                            string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                            // Save the message as .msg
                            try
                            {
                                message.Save(msgPath);
                                Console.WriteLine($"    Saved to: {msgPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"    Error saving message: {ex.Message}");
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
