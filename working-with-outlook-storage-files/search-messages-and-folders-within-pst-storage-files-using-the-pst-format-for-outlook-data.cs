using Aspose.Email.Tools.Search;
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

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Display total items count.
                int totalItems = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items in PST: {totalItems}");

                // Iterate through all subfolders of the root folder.
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folder.DisplayName}");
                    Console.WriteLine($"  Items: {folder.ContentCount}");
                    Console.WriteLine($"  Unread: {folder.ContentUnreadCount}");

                    // Example: search for messages with subject containing "Invoice".
                    PersonalStorageQueryBuilder queryBuilder = new PersonalStorageQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery query = queryBuilder.GetQuery();

                    foreach (MessageInfo msgInfo in folder.EnumerateMessages(query))
                    {
                        Console.WriteLine($"    Message Subject: {msgInfo.Subject}");
                        Console.WriteLine($"    From: {msgInfo.SenderRepresentativeName}");
                        Console.WriteLine($"    To: {msgInfo.DisplayTo}");
                    }
                }

                // Example: search for folders whose display name contains "Archive".
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    if (folder.DisplayName != null && folder.DisplayName.IndexOf("Archive", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"Found archive folder: {folder.DisplayName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
