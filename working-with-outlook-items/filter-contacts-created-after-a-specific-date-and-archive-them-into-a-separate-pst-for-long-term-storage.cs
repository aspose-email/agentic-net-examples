using Aspose.Email.PersonalInfo;
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
            // Paths for source and archive PST files
            string sourcePstPath = "source.pst";
            string archivePstPath = "archive.pst";

            // Date filter – contacts created after this date will be archived
            DateTime filterDate = new DateTime(2023, 1, 1);

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Source PST file not found: {sourcePstPath}");
                return;
            }

            // If an old archive PST exists, delete it to start fresh
            if (File.Exists(archivePstPath))
            {
                try
                {
                    File.Delete(archivePstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unable to delete existing archive PST: {ex.Message}");
                    return;
                }
            }

            // Create a new archive PST (Unicode format)
            using (PersonalStorage archivePst = PersonalStorage.Create(archivePstPath, FileFormatVersion.Unicode))
            {
                // Create a Contacts folder in the archive PST
                FolderInfo archiveContactsFolder = archivePst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);

                // Open the source PST for reading
                using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
                {
                    // Get the Contacts folder from the source PST
                    FolderInfo sourceContactsFolder = sourcePst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                    // Enumerate all messages (contacts) in the source Contacts folder
                    foreach (MessageInfo messageInfo in sourceContactsFolder.EnumerateMessages())
                    {
                        using (MapiMessage contactMessage = sourcePst.ExtractMessage(messageInfo))
                        {
                            // Ensure the item is a contact
                            if (contactMessage.MessageClass != null && contactMessage.MessageClass.Equals("IPM.Contact", StringComparison.OrdinalIgnoreCase))
                            {
                                // Use ClientSubmitTime as the creation timestamp
                                DateTime? creationTime = contactMessage.ClientSubmitTime;
                                if (creationTime.HasValue && creationTime.Value > filterDate)
                                {
                                    // Add the contact to the archive PST
                                    archiveContactsFolder.AddMessage(contactMessage);
                                }
                            }
                        }
                    }
                }

                // Save changes (implicit on dispose)
                Console.WriteLine($"Archive PST created at: {archivePstPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
