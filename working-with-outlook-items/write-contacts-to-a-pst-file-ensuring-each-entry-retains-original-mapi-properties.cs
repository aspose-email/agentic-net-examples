using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "contacts.pst";

            // Ensure the directory for the PST file exists
            string directory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Open existing PST or create a new one if it does not exist
            PersonalStorage pst = File.Exists(pstPath)
                ? PersonalStorage.FromFile(pstPath)
                : PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);

            using (pst)
            {
                // Get the predefined Contacts folder (creates it if missing)
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                // Create a new contact and set standard properties
                using (MapiContact contact = new MapiContact())
                {
                    contact.NameInfo.GivenName = "John";
                    contact.NameInfo.Surname = "Doe";
                    contact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

                    // Add a custom MAPI property (example tag 0x8000 with Unicode string)
                    byte[] customData = Encoding.Unicode.GetBytes("CustomValue");
                    MapiProperty customProp = new MapiProperty(0x8000, customData);
                    contact.SetProperty(customProp);

                    // Add the contact to the PST Contacts folder
                    contactsFolder.AddMapiMessageItem(contact);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
