using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstFilePath = "contacts.pst";

            // Verify PST file existence
            if (!File.Exists(pstFilePath))
            {
                // Create a minimal placeholder PST if missing
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
                return;
            }

            // Open PST and perform a basic integrity check
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                try
                {
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        // Access a property to force enumeration and surface errors
                        int _ = folderInfo.ContentCount;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"PST integrity check failed: {ex.Message}");
                    return;
                }

                // Retrieve the Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in PST.");
                    return;
                }

                // Enumerate and display contacts
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        Console.WriteLine($"Contact entry: Subject = {mapiMessage.Subject}");
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
