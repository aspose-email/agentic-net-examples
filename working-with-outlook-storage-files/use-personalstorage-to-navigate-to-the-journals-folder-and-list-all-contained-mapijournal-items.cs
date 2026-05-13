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
            // Path to the PST file (placeholder - replace with actual path if needed)
            string pstPath = "sample.pst";

            // Ensure a PST file exists; create an empty one if it does not.
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Empty PST created.
                }
                Console.WriteLine($"Created empty PST file at: {pstPath}");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Journal folder (standard IPM folder for journals)
                FolderInfo journalFolder = pst.GetPredefinedFolder(StandardIpmFolder.Journal);

                // Enumerate all items in the Journal folder
                foreach (MessageInfo msgInfo in journalFolder.GetContents())
                {
                    // Extract the message as a MapiMessage
                    MapiMessage message = pst.ExtractMessage(msgInfo);

                    // Check if the item is a journal based on its MessageClass
                    if (!string.IsNullOrEmpty(message.MessageClass) &&
                        message.MessageClass.StartsWith("IPM.Journal", StringComparison.OrdinalIgnoreCase))
                    {
                        // Output selected journal properties
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"Message Class: {message.MessageClass}");
                        Console.WriteLine(new string('-', 40));
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
