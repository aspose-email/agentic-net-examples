using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                // Verify that the PST file exists before attempting to open it
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Define the keywords to search for in the subject
                    List<string> keywords = new List<string>();
                    keywords.Add("Project");
                    keywords.Add("Report");

                    // Start processing from the root folder
                    ProcessFolder(pst.RootFolder, keywords);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Recursively processes a folder and its subfolders
        private static void ProcessFolder(FolderInfo folder, List<string> keywords)
        {
            // Enumerate messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                bool matchesAll = true;

                // Check each keyword against the message subject
                foreach (string keyword in keywords)
                {
                    if (messageInfo.Subject == null ||
                        messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        matchesAll = false;
                        break;
                    }
                }

                // If the subject contains all keywords, output it
                if (matchesAll)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }

            // Recurse into subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, keywords);
            }
        }
    }
}
