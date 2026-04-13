using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default Contacts folder to make the PST usable
                        placeholder.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open PST and perform integrity check
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    bool isCorrupted = false;

                    // Attempt to enumerate subfolders; any exception indicates corruption
                    try
                    {
                        foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                        {
                            // No operation needed; just iterating validates structure
                            foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                            {
                                // Access a property to ensure message info is readable
                                string subject = msgInfo.Subject;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"PST integrity check failed: {ex.Message}");
                        isCorrupted = true;
                    }

                    if (isCorrupted)
                    {
                        Console.WriteLine("The PST file appears to be corrupted. Aborting contact loading.");
                        return;
                    }

                    Console.WriteLine("PST integrity check passed. Loading contacts...");

                    // Load contacts from the predefined Contacts folder
                    FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                    foreach (MessageInfo contactInfo in contactsFolder.EnumerateMessages())
                    {
                        using (MapiMessage contactMessage = pst.ExtractMessage(contactInfo))
                        {
                            Console.WriteLine($"Contact: {contactMessage.Subject}");
                            // Additional contact processing can be added here
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
