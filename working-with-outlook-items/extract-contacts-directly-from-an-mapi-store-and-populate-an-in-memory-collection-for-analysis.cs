using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file
            string pstPath = "contacts.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a predefined Contacts folder
                        createdPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                    return;
                }
            }

            // Collection to hold extracted contacts
            List<MapiContact> contacts = new List<MapiContact>();

            // Open the PST file and extract contacts
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the Contacts folder; if it does not exist, skip extraction
                    FolderInfo contactsFolder;
                    try
                    {
                        contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Contacts folder not found in the PST.");
                        return;
                    }

                    // Enumerate all messages in the Contacts folder
                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Verify the message is a contact item
                            if (mapiMessage.SupportedType == MapiItemType.Contact)
                            {
                                // Convert to MapiContact
                                MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem();
                                contacts.Add(contact);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }

            // Output analysis results
            Console.WriteLine($"Total contacts extracted: {contacts.Count}");
            foreach (MapiContact contact in contacts)
            {
                Console.WriteLine($"Name: {contact.NameInfo.DisplayName}");
                Console.WriteLine($"Email: {contact.ElectronicAddresses.Email1?.EmailAddress}");
                Console.WriteLine($"Company: {contact.ProfessionalInfo?.CompanyName}");
                Console.WriteLine("---");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
