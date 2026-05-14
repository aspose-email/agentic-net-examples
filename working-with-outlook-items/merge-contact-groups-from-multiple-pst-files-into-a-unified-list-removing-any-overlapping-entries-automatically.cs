using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define source PST files and target merged PST file
            string[] sourcePstFiles = new string[] { "contacts1.pst", "contacts2.pst", "contacts3.pst" };
            string targetPstPath = "merged_contacts.pst";

            // Verify source files exist; skip any missing files
            List<string> existingSources = new List<string>();
            foreach (string sourcePath in sourcePstFiles)
            {
                if (File.Exists(sourcePath))
                {
                    existingSources.Add(sourcePath);
                }
                else
                {
                    Console.Error.WriteLine($"Source PST not found and will be skipped: {sourcePath}");
                }
            }

            if (existingSources.Count == 0)
            {
                Console.Error.WriteLine("No valid source PST files were found. Exiting.");
                return;
            }

            // Ensure the target PST file exists; create a minimal PST if it does not
            if (!File.Exists(targetPstPath))
            {
                try
                {
                    PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create target PST file: {ex.Message}");
                    return;
                }
            }

            // Open the target PST and merge the existing source PSTs
            try
            {
                using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
                {
                    try
                    {
                        targetPst.MergeWith(existingSources.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Merging PST files failed: {ex.Message}");
                        return;
                    }

                    // Access the predefined Contacts folder
                    FolderInfo contactsFolder;
                    try
                    {
                        contactsFolder = targetPst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unable to retrieve Contacts folder: {ex.Message}");
                        return;
                    }

                    // Collect unique contacts based on display name
                    HashSet<string> uniqueContactNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    List<MapiMessage> uniqueContacts = new List<MapiMessage>();

                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        try
                        {
                            using (MapiMessage contactMessage = targetPst.ExtractMessage(messageInfo))
                            {
                                string displayName = contactMessage.Subject ?? string.Empty;
                                if (!uniqueContactNames.Contains(displayName))
                                {
                                    uniqueContactNames.Add(displayName);
                                    // Clone the message to keep it after disposing the original
                                    MapiMessage cloned = contactMessage.Clone();
                                    uniqueContacts.Add(cloned);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to process a contact message: {ex.Message}");
                            // Continue processing other messages
                        }
                    }

                    // Output the unified contact list
                    Console.WriteLine("Unified Contact List:");
                    foreach (MapiMessage contact in uniqueContacts)
                    {
                        Console.WriteLine($"- {contact.Subject}");
                        // Dispose cloned messages after use
                        contact.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error handling target PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
