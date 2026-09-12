using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file
            const string pstPath = "storage.pst";

            // Verify that the PST file exists before proceeding
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Directory where extracted messages will be saved
            const string outputDir = "ExtractedMessages";

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDir);

            // Keywords to search for in message subjects (case‑insensitive)
            string[] keywords = { "invoice", "report", "meeting" };

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Iterate through each message in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        // Guard against null subjects
                        if (string.IsNullOrEmpty(messageInfo.Subject))
                            continue;

                        // Check if the subject contains any of the keywords
                        bool matches = keywords.Any(k =>
                            messageInfo.Subject.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

                        if (!matches)
                            continue;

                        // Extract the full message object as a MapiMessage
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        // Create a safe filename from the subject
                        string safeSubject = string.Concat(messageInfo.Subject.Split(Path.GetInvalidFileNameChars()));
                        if (string.IsNullOrWhiteSpace(safeSubject))
                            safeSubject = "Untitled";

                        // Optionally limit filename length to avoid filesystem limits
                        const int maxFileNameLength = 100;
                        if (safeSubject.Length > maxFileNameLength)
                            safeSubject = safeSubject.Substring(0, maxFileNameLength);

                        string outputPath = Path.Combine(outputDir, safeSubject + ".msg");

                        // Save the message as a .msg file
                        msg.Save(outputPath);
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
