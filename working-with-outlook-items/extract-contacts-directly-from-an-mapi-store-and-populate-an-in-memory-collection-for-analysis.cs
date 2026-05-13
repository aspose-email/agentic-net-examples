using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file that contains contacts
            string pstPath = "contacts.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
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
                    // Get the predefined Contacts folder
                    FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                    // Enumerate all messages in the Contacts folder
                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        // Extract the full MAPI message
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Process only contact items
                            if (mapiMessage.SupportedType == MapiItemType.Contact)
                            {
                                // Convert the MAPI message to a MapiContact
                                MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem();

                                // Add to the in‑memory collection
                                contacts.Add(contact);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while processing PST file: {ex.Message}");
                return;
            }

            // Output basic information about the extracted contacts
            Console.WriteLine($"Total contacts extracted: {contacts.Count}");
            foreach (MapiContact contact in contacts)
            {
                // Example: display name and primary email address (if available)
                string displayName = contact.NameInfo?.DisplayName ?? "(no name)";
                string email = contact.ElectronicAddresses?.Email1?.EmailAddress ?? "(no email)";
                Console.WriteLine($"Name: {displayName}, Email: {email}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
