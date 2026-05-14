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
        const string pstPath = "contacts.pst";

        // Ensure a PST file exists – create a minimal one with a Contacts folder if missing
        if (!File.Exists(pstPath))
        {
            CreateSamplePst(pstPath);
        }

        try
        {
            // Open PST in read‑only mode
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Try to get the Contacts folder
                FolderInfo contactsFolder;
                try
                {
                    contactsFolder = pst.RootFolder.GetSubFolder("Contacts");
                }
                catch
                {
                    Console.Error.WriteLine("Contacts folder not found in PST.");
                    return;
                }

                foreach (MessageInfo msgInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                    {
                        // Process only contact items
                        if (mapiMsg.SupportedType != MapiItemType.Contact)
                            continue;

                        // Convert to a strongly typed contact object
                        MapiContact contact = (MapiContact)mapiMsg.ToMapiMessageItem();

                        // Display read‑only audit information
                        Console.WriteLine($"Display Name: {contact.NameInfo?.DisplayName}");
                        string email = contact.ElectronicAddresses?.Email1?.EmailAddress;
                        if (!string.IsNullOrEmpty(email))
                            Console.WriteLine($"Email: {email}");
                        Console.WriteLine($"Company: {contact.ProfessionalInfo?.CompanyName}");
                        Console.WriteLine("---");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateSamplePst(string path)
    {
        // Create a new PST file with an empty Contacts folder
        using (PersonalStorage pst = PersonalStorage.Create(path, FileFormatVersion.Unicode))
        {
            pst.RootFolder.AddSubFolder("Contacts");
        }
    }
}
