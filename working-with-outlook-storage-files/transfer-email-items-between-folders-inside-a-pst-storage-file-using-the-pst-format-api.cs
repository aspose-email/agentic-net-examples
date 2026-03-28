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

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Locate the source folder (e.g., Inbox)
                FolderInfo sourceFolder;
                try
                {
                    sourceFolder = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to locate source folder – {ex.Message}");
                    return;
                }

                // Locate or create the destination folder (e.g., Archive)
                FolderInfo destinationFolder;
                try
                {
                    destinationFolder = pst.RootFolder.GetSubFolder("Archive");
                }
                catch (Exception)
                {
                    try
                    {
                        destinationFolder = pst.RootFolder.AddSubFolder("Archive");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Unable to create destination folder – {ex.Message}");
                        return;
                    }
                }

                // Move each message from source to destination
                foreach (MessageInfo messageInfo in sourceFolder.EnumerateMessages())
                {
                    try
                    {
                        pst.MoveItem(messageInfo, destinationFolder);
                        Console.WriteLine($"Moved message: {messageInfo.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error moving message '{messageInfo.Subject}': {ex.Message}");
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
