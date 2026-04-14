using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePstPath1 = "Contacts1.pst";
            string sourcePstPath2 = "Contacts2.pst";
            string destPstPath = "MergedContacts.pst";

            // Verify source files exist
            if (!File.Exists(sourcePstPath1))
            {
                Console.Error.WriteLine($"Source PST not found: {sourcePstPath1}");
                return;
            }
            if (!File.Exists(sourcePstPath2))
            {
                Console.Error.WriteLine($"Source PST not found: {sourcePstPath2}");
                return;
            }

            // Ensure destination directory exists
            string destDirectory = Path.GetDirectoryName(destPstPath);
            if (!string.IsNullOrEmpty(destDirectory) && !Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            // If destination PST already exists, delete it to start fresh
            if (File.Exists(destPstPath))
            {
                try
                {
                    File.Delete(destPstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete existing destination PST: {ex.Message}");
                    return;
                }
            }

            // Create destination PST (Unicode format)
            using (PersonalStorage destPst = PersonalStorage.Create(destPstPath, FileFormatVersion.Unicode))
            {
                // Get or create the Contacts folder in the destination PST
                FolderInfo destContactsFolder = destPst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (destContactsFolder == null)
                {
                    destContactsFolder = destPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                }

                // HashSet to track already imported contact subjects (names)
                HashSet<string> importedContactNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Local function to import contacts from a source PST
                void ImportContacts(string sourcePath)
                {
                    using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePath))
                    {
                        FolderInfo sourceContactsFolder = sourcePst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                        if (sourceContactsFolder == null)
                        {
                            Console.WriteLine($"No contacts folder found in {sourcePath}");
                            return;
                        }

                        foreach (MessageInfo msgInfo in sourceContactsFolder.EnumerateMessages())
                        {
                            // Use the subject (usually the contact's display name) as a simple duplicate key
                            string contactName = msgInfo.Subject ?? string.Empty;
                            if (importedContactNames.Contains(contactName))
                            {
                                continue; // Skip duplicate
                            }

                            using (MapiMessage contactMessage = sourcePst.ExtractMessage(msgInfo))
                            {
                                // Add the contact message to the destination contacts folder
                                destContactsFolder.AddMessage(contactMessage);
                            }

                            importedContactNames.Add(contactName);
                        }
                    }
                }

                // Import contacts from both source PSTs
                ImportContacts(sourcePstPath1);
                ImportContacts(sourcePstPath2);

                // No explicit save needed; disposing the PersonalStorage writes the file
                Console.WriteLine($"Merged contacts saved to {destPstPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
