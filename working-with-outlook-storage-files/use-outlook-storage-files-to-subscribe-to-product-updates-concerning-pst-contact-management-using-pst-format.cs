using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file containing contacts
            string pstPath = "contacts.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found at path: {pstPath}");
                return;
            }

            // Directory where extracted contacts will be saved
            string outputDir = "output";
            Directory.CreateDirectory(outputDir); // Ensure the directory exists

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in the PST.");
                    return;
                }

                Console.WriteLine($"Folder: {contactsFolder.DisplayName}");
                Console.WriteLine($"Total items: {contactsFolder.ContentCount}");
                Console.WriteLine($"Total unread items: {contactsFolder.ContentUnreadCount}");

                // Enumerate each contact message
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    // Extract the full message (contact) from the PST
                    MapiMessage contactMessage = pst.ExtractMessage(messageInfo);

                    // Output basic contact information
                    Console.WriteLine($"Contact Subject (Name): {contactMessage.Subject}");

                    // Prepare a safe filename
                    string safeFileName = string.IsNullOrWhiteSpace(contactMessage.Subject)
                        ? "UnnamedContact"
                        : contactMessage.Subject;

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeFileName = safeFileName.Replace(c, '_');
                    }

                    string outputPath = Path.Combine(outputDir, $"{safeFileName}.msg");

                    try
                    {
                        contactMessage.Save(outputPath);
                        Console.WriteLine($"Saved contact to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact '{safeFileName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
