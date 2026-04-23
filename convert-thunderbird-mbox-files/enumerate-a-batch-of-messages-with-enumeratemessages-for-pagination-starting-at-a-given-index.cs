using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    // Add a default Inbox folder
                    using (PersonalStorage pstCreate = PersonalStorage.FromFile(pstPath))
                    {
                        pstCreate.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST and enumerate a batch of messages
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve the Inbox folder
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Define pagination parameters
                int startIndex = 0;   // zero‑based start index
                int count = 10;       // number of messages to retrieve

                // Enumerate messages with pagination
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages(startIndex, count))
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
