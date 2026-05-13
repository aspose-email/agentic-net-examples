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
            const string pstPath = "sample.pst";
            const string password = "secret";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST file.
            PersonalStorage pst = null;
            try
            {
                pst = PersonalStorage.FromFile(pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Verify password if the PST is protected.
                if (pst.Store.IsPasswordProtected)
                {
                    if (!pst.Store.IsPasswordValid(password))
                    {
                        Console.Error.WriteLine("Invalid PST password.");
                        return;
                    }
                }

                // Enumerate all folders and messages.
                try
                {
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName} (Items: {folder.ContentCount})");
                        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                        {
                            Console.WriteLine($"  Subject: {msgInfo.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while enumerating messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
