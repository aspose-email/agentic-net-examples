using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "input.pst";
            string outputDir = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a standard Inbox folder to satisfy PST structure
                        placeholder.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the root folder and iterate through all subfolders
                    Queue<FolderInfo> folders = new Queue<FolderInfo>();
                    folders.Enqueue(pst.RootFolder);

                    while (folders.Count > 0)
                    {
                        FolderInfo currentFolder = folders.Dequeue();

                        // Enqueue subfolders for later processing
                        foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                        {
                            folders.Enqueue(subFolder);
                        }

                        // Process each message in the current folder
                        foreach (MessageInfo msgInfo in currentFolder.EnumerateMessages())
                        {
                            try
                            {
                                using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                                {
                                    // Check if the message is a contact
                                    if (msg.SupportedType == MapiItemType.Contact)
                                    {
                                        // Convert to MapiContact
                                        MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                                        // Preserve categories (already part of the contact)
                                        // Save as VCard file; file name based on display name or subject
                                        string safeName = string.IsNullOrWhiteSpace(contact.NameInfo.DisplayName)
                                            ? "Contact"
                                            : contact.NameInfo.DisplayName.Replace(Path.GetInvalidFileNameChars(), '_');
                                        string vcfPath = Path.Combine(outputDir, $"{safeName}.vcf");

                                        // Save the contact as VCard
                                        contact.Save(vcfPath, ContactSaveFormat.VCard);
                                    }
                                }
                            }
                            catch (Exception msgEx)
                            {
                                Console.Error.WriteLine($"Failed to process message ID {msgInfo.EntryId}: {msgEx.Message}");
                                // Continue with next message
                            }
                        }
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Failed to open PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

// Extension method to replace invalid filename characters
static class StringExtensions
{
    public static string Replace(this string str, char[] chars, char replacement)
    {
        foreach (char c in chars)
        {
            str = str.Replace(c, replacement);
        }
        return str;
    }
}
