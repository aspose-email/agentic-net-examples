using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstFilePath = "contacts.pst";
            string outputFolder = "vcards";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstFilePath))
            {
                using (PersonalStorage placeholderPst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    placeholderPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                }
                Console.WriteLine($"Placeholder PST created at {pstFilePath}");
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in PST.");
                    return;
                }

                // Enumerate all messages in the Contacts folder
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Process only contact items
                        if (mapiMessage.SupportedType == MapiItemType.Contact)
                        {
                            // Convert to MapiContact
                            using (MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem())
                            {
                                // Determine a safe file name for the vCard
                                string displayName = contact.NameInfo != null ? contact.NameInfo.DisplayName : null;
                                string safeName = string.IsNullOrEmpty(displayName) ? Guid.NewGuid().ToString() : SanitizeFileName(displayName);
                                string vcfPath = Path.Combine(outputFolder, safeName + ".vcf");

                                // Save the contact as a vCard file
                                contact.Save(vcfPath);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Replaces invalid filename characters with an underscore
    private static string SanitizeFileName(string fileName)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }
        return fileName;
    }
}
