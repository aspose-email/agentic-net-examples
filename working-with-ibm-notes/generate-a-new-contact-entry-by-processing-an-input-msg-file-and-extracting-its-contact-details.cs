using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "contact.msg";
            string outputVcfPath = "newContact.vcf";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Ensure the MSG is a contact
                if (msg.SupportedType != MapiItemType.Contact)
                {
                    Console.Error.WriteLine("Error: The provided MSG file is not a contact.");
                    return;
                }

                // Convert to MapiContact
                MapiContact sourceContact = (MapiContact)msg.ToMapiMessageItem();

                // Create a new contact and copy desired fields
                using (MapiContact newContact = new MapiContact())
                {
                    // Copy display name
                    newContact.NameInfo.DisplayName = sourceContact.NameInfo.DisplayName;

                    // Copy primary email address if present
                    if (sourceContact.ElectronicAddresses?.Email1 != null &&
                        !string.IsNullOrEmpty(sourceContact.ElectronicAddresses.Email1.EmailAddress))
                    {
                        newContact.ElectronicAddresses.Email1 = new MapiContactElectronicAddress(sourceContact.ElectronicAddresses.Email1.EmailAddress);
                    }

                    // Save the new contact as a vCard file
                    try
                    {
                        newContact.Save(outputVcfPath);
                        Console.WriteLine($"New contact saved to {outputVcfPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving contact: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
