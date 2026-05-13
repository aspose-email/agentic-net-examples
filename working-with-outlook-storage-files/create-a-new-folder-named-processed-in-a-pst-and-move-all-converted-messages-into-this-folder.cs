using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get or create the "Processed" folder
                FolderInfo processedFolder = pst.RootFolder.GetSubFolder("Processed");
                if (processedFolder == null)
                {
                    try
                    {
                        processedFolder = pst.RootFolder.AddSubFolder("Processed");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create 'Processed' folder: {ex.Message}");
                        return;
                    }
                }

                // Move all messages from the root folder to the "Processed" folder
                foreach (MessageInfo messageInfo in pst.RootFolder.EnumerateMessages())
                {
                    try
                    {
                        pst.MoveItem(messageInfo, processedFolder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {ex.Message}");
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
